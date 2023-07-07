using Core;
using Fight;
using UnityEngine;
using TMPro;
using Core.EventSystem;

public class FT_FightStatsUI : MonoBehaviour
{
    [SerializeField] private GameObject m_scoreboard;
    [SerializeField] private GameObject m_winbanner;
    [SerializeField] private GameObject m_losebanner;
    [SerializeField] private GameObject m_fightOverCard;

    [Header("FightEndCard")]
    [SerializeField] private TextMeshProUGUI XpPoints;
    [SerializeField] private TextMeshProUGUI SkillPoints;
    [SerializeField] private TextMeshProUGUI FishBucks;

    public void FightEndCard(int winnerID, int aXpPoints, int aSkillPts, int aFishBucks)
    {
        FT_Core.isAnyCanvasActive = true;

        if (winnerID == FightUtil.GetPlayer().GetID()) 
            m_winbanner.SetActive(true);
        else 
            m_losebanner.SetActive(true);

        //display rewards on endcard
        XpPoints.text = aXpPoints.ToString() + " XP!";
        SkillPoints.text = aSkillPts.ToString() + " SKill Points";
        FishBucks.text = aFishBucks.ToString();

        GameUtil.TriggerTimer.CreateTimer(() =>
        {
            GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
            GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
            
            m_winbanner.SetActive(false);
            m_losebanner.SetActive(false);
            m_fightOverCard.SetActive(true);
        }, 2.0f);

    }

    public void ReturnToMap()
    {
        GameUtil.EventManager.AddEvent(new KSB_EndSceneEvent("IM_Scene", GameUtil.MapCore, null));
    }
}
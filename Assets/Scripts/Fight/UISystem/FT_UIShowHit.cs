using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.UISystem
{
    public class FT_UIShowHit : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerHitEffect;
    [SerializeField] private GameObject m_AIHitEffect;
    [SerializeField] public GameObject m_Canvas;

    public void ShowHit(int damageID)
    {
        if (damageID == FightUtil.GetPlayer().GetID()) { m_PlayerHitEffect.SetActive(true); }
        else { m_AIHitEffect.SetActive(true); }
        //m_HitEffect.SetActive(true);

        GameUtil.TriggerTimer.CreateTimer(() =>
        {
            m_PlayerHitEffect.SetActive(false);
            m_AIHitEffect.SetActive(false);
            
        }, 1.0f);
        //if (damageID == FightUtil.GetPlayer().GetID())
        //{
        //    GameObject PlayerHit = Instantiate(m_PlayerHitEffect, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        //    PlayerHit.transform.SetParent(m_Canvas.transform,false);
        //    PlayerHit.transform.localScale = new Vector3(1, 1, 1);
        //    PlayerHit.SetActive(true);
        //}
        //else
        //{
        //    GameObject AIHit = Instantiate(m_AIHitEffect, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        //    AIHit.transform.SetParent(m_Canvas.transform,false);
        //    AIHit.transform.localScale = new Vector3(1, 1, 1);
        //    AIHit.SetActive(true);
        //}

    }
}
}

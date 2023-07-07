using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.EventSystem;
using PublicEvents;

public class IM_EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text FishBuckText;
    [SerializeField] private TMP_Text PlayerLevelText;
    [SerializeField] private Slider ExpBarSlider;

    private void Start()
    {
        GameUtil.EventManager.RegisterSubscriber(HandleAddXpPointsEvent, AddPlayerXpPointsEvent.EventID);

        FishBuckText.text = GameUtil.PlayerState.FishBucks.ToString();

        PlayerLevelText.text = GameUtil.PlayerState.GetPlayerLevel().ToString();

        SetMaxExpPoints(GameUtil.PlayerState.MaxExperiencePoints);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            AddPlayerExp(GameUtil.PlayerState.GetPlayerLevel() * 3);
        }

        FishBuckText.text = GameUtil.PlayerState.FishBucks.ToString();
        SetExpPoints(GameUtil.PlayerState.ExperiencePoints);
    }

    private void SetExpPoints(int points) => ExpBarSlider.value = points;
    
    private void SetMaxExpPoints(int points) => ExpBarSlider.maxValue = points;

    public void AddPlayerExp(int points)
    {
        GameUtil.PlayerState.ExperiencePoints += points;

        int CurrentPoints = GameUtil.PlayerState.ExperiencePoints;
        int MaxPoints = GameUtil.PlayerState.MaxExperiencePoints;

        if (CurrentPoints >= MaxPoints)
        {
            int remainingPoints = MaxPoints - CurrentPoints;

            GameUtil.PlayerState.IncrementPlayerLevel();

            GameUtil.PlayerState.ExperiencePoints = 0;
            GameUtil.PlayerState.ExperiencePoints -= remainingPoints;

            PlayerLevelText.text = GameUtil.PlayerState.GetPlayerLevel().ToString();

            SetMaxExpPoints(GameUtil.PlayerState.MaxExperiencePoints);
        }
        SetExpPoints(GameUtil.PlayerState.ExperiencePoints);
    }

    public void HandleAddXpPointsEvent(KSB_IEvent gameEvent)
    {
        AddPlayerXpPointsEvent xpEvent = (AddPlayerXpPointsEvent) gameEvent;
        AddPlayerExp(xpEvent.XpPoints);
    }
}

using Core.EventSystem;
using Fight.Events;
using UnityEngine;

namespace Fight.UISystem
{
public class FT_UIManager: MonoBehaviour
{
    [SerializeField] private FT_FighterUI[] m_FighterUIs;

    [SerializeField] private FT_FightStatsUI m_UIFightStats;
    [SerializeField] private FT_UICountDown m_UICountDown;
    [SerializeField] private FT_UIItemDisplay m_UIItemDisplay;
    [SerializeField] private FT_UIShowHit m_UIShowHit;

    public void Awake()
    {
        GameUtil.EventManager.RegisterSubscriber(HandleHealthChange, FT_ChangeHealthEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleStaminaChange, FT_ChangeStaminaEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleShowDamage, FT_ShowDamageUIEvent.EventID);

        GameUtil.EventManager.RegisterSubscriber(HandleRoundBeginEvent, FT_RoundBeginEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleRoundEndEvent, FT_RoundOverEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleFightOverEvent, FT_FightOverEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleItemUsedEvent, FT_ItemUsedEvent.EventID);

        m_UIItemDisplay.Initialize(GameUtil.PlayerState.LoadoutItems);
    }

    public void StartFight()
    {
        m_UICountDown.StartFight();
    }

    void HandleRoundBeginEvent(KSB_IEvent aEvent)
    {
        FT_RoundBeginEvent roundEvent = (FT_RoundBeginEvent)aEvent;

        m_FighterUIs[0].SetActive(true);
        m_FighterUIs[1].SetActive(true);
    }

    void HandleHealthChange(KSB_IEvent aEvent)
    {
        FT_ChangeHealthEvent UIEvent = (FT_ChangeHealthEvent)aEvent;
        m_FighterUIs[UIEvent.Fighter.GetID()].UpdateHealthBar(UIEvent.ChangeAmount, UIEvent.Fighter.GetMaxHealth());
    }

    void HandleShowDamage(KSB_IEvent aEvent)
    {
        FT_ShowDamageUIEvent UIEvent = (FT_ShowDamageUIEvent)aEvent;
        m_UIShowHit.ShowHit(FightUtil.GetOpponent(UIEvent.Fighter.GetID()).GetID());
        
    }
    void HandleStaminaChange(KSB_IEvent aEvent)
    {
         FT_ChangeStaminaEvent UIEvent = (FT_ChangeStaminaEvent)aEvent;
         m_FighterUIs[UIEvent.Fighter.GetID()].UpdateStaminaBar(UIEvent.ChangeAmount, UIEvent.Fighter.GetMaxStamina());
    }

    void HandleRoundEndEvent(KSB_IEvent aEvent)
    {
        FT_RoundOverEvent roundEndEvent = (FT_RoundOverEvent)aEvent;
        m_FighterUIs[roundEndEvent.Winner.GetID()].IncrementWinCount();
    }

    private void HandleFightOverEvent(KSB_IEvent gameEvent)
    {
        FT_FightOverEvent fightOverEvent = (FT_FightOverEvent) gameEvent;
        m_FighterUIs[fightOverEvent.WinnerFighterId].IncrementWinCount();

        if (fightOverEvent.WinnerFighterId == FightUtil.GetPlayerID())
        {
            m_UIFightStats.FightEndCard(fightOverEvent.WinnerFighterId, FightUtil.CoreData.WinSkillPoints, FightUtil.CoreData.WinSkillPoints, FightUtil.CoreData.WinFishBucks);
        }
        else
        {
            m_UIFightStats.FightEndCard(fightOverEvent.WinnerFighterId, FightUtil.CoreData.LoseSkillPoints, FightUtil.CoreData.LoseSkillPoints, FightUtil.CoreData.LoseFishBucks);
        }
    }

    private void HandleItemUsedEvent(KSB_IEvent gameEvent)
    {
        FT_ItemUsedEvent itemUsedEvent = (FT_ItemUsedEvent)gameEvent;
        m_UIItemDisplay.ItemUsed(itemUsedEvent.ItemIndex,itemUsedEvent.ItemQuantity);
    }

    public void UnRegisterToEventSystem()
    {
        GameUtil.EventManager.UnRegisterSubscribers(
            HandleRoundBeginEvent,
            HandleRoundEndEvent,
            HandleFightOverEvent,
            HandleHealthChange,
            HandleStaminaChange,
            HandleShowDamage,
            HandleItemUsedEvent
        );
    }
    }
}
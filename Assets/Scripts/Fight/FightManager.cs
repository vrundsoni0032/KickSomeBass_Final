using Core.EventSystem;
using Fight.Events;
using System.Collections.Generic;
using UnityEngine;
using PublicEvents;

namespace Fight
{
public class FT_FightManager
{
    private readonly KSB_IEventManager m_eventManager;

    private int m_currentRound = 0;
    private readonly int m_totalRounds;

    private readonly Dictionary<int, int> m_winCount; //ID, winCount

    private bool m_RoundInProcess;

    private int m_WinPoints;
    private int m_LosePoints;

    public int m_WinSkillPoints;
    public int m_LoseSkillPoints;
               
    public int m_WinFishBucks;
    public int m_LoseFishBucks;

    public FT_FightManager(int totalRounds, int playerId, int aiId, FT_CoreData coreData)
    {
        m_eventManager = GameUtil.EventManager;
        m_eventManager.RegisterSubscriber(HandleFighterLostEvent, FT_FighterLostEvent.EventID);
        m_eventManager.RegisterSubscriber(HandleRoundBeginEvent, FT_RoundBeginEvent.EventID);
        m_eventManager.RegisterSubscriber(HandleRoundOverEvent, FT_RoundOverEvent.EventID);

        m_totalRounds = totalRounds;

        m_winCount = new Dictionary<int, int>
        {
            { playerId, 0 },
            { aiId, 0 }
        };

        switch (coreData.FightIndex)
        {
            case FightIndex.TutorialFight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.AttemptedFirstFight);
                break;

            case FightIndex.Area2Fight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.AttemptedSecondFight);
                break;

            case FightIndex.Area3Fight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.AttemptedThirdFight);
                break;

            case FightIndex.LighthouseFight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.AttemptedFourthFight);
                break;
        }
        }

    private void HandleRoundOverEvent(KSB_IEvent gameEvent)
    {
        // Reset fighter and AI Location for the start of the next round.
        Vector3[] arenaSpawnPoints = FightUtil.Arena.GetRandomSpawnPoints();
        FightUtil.GetPlayer().transform.position = arenaSpawnPoints[0];
        FightUtil.GetAI().transform.position = arenaSpawnPoints[1];

    }
    private void HandleRoundBeginEvent(KSB_IEvent gameEvent)
    {
        m_RoundInProcess = true;

        // Enable fighter movement.
        m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), false, false, false, false));
        m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(FightUtil.GetAI(), false, false, false, false));

        // Reset Heath and Stamina.
        m_eventManager.AddEvent(new FT_ChangeHealthEvent(FightUtil.GetPlayer(), FightUtil.GetPlayer().GetMaxHealth()));
        m_eventManager.AddEvent(new FT_ChangeStaminaEvent(FightUtil.GetPlayer(), FightUtil.GetPlayer().GetMaxStamina()));

        m_eventManager.AddEvent(new FT_ChangeHealthEvent(FightUtil.GetAI(), FightUtil.GetAI().GetMaxHealth()));
        m_eventManager.AddEvent(new FT_ChangeStaminaEvent(FightUtil.GetAI(), FightUtil.GetAI().GetMaxStamina()));
    }

    private void HandleFighterLostEvent(KSB_IEvent aEvent)
    {
        FT_FighterLostEvent fighterLostEvent = (FT_FighterLostEvent)aEvent;

        int playerId = FightUtil.GetPlayer().GetID();
        int aiId = FightUtil.GetAI().GetID();

        if(!m_RoundInProcess)
            return;

        m_RoundInProcess = false;
        m_currentRound++;
        int winnerId = fighterLostEvent.Winner.GetID();
        m_winCount[winnerId]++;

        // Disable fighter movement.
        m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), true, true, true, true));
        m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(FightUtil.GetAI(), true, true, true, true));

        if (m_currentRound > m_totalRounds || m_winCount[winnerId] > Mathf.Floor(m_totalRounds / 2.0f))
        {
            int fightWinnerId = (m_winCount[playerId] > m_winCount[aiId]) ? playerId : aiId;

            DistributePlayerRewards(fightWinnerId);
            SetPlayerStoryEvent(fightWinnerId);

            m_eventManager.AddEvent(new FT_FightOverEvent(fightWinnerId));

            GameUtil.SoundManager.PlaySound("LevelUp");

            return;
        }
        m_eventManager.AddEvent(new FT_RoundOverEvent(fighterLostEvent.Loser));
    }
    
    //This 3 funcs should only be called once in the fight manager during the initialization
    public void SetWinLoseXpPoints(int aWinpts, int aLosePts)
    {
        m_WinPoints = aWinpts;
        m_LosePoints = aLosePts;
    }

    public void SetWinLoseSkillPoints(int aWinpts, int aLosePts)
    {
        m_WinSkillPoints = aWinpts;
        m_LoseSkillPoints = aLosePts;
    }

    public void SetWinLoseFishBucks(int aWinBcks, int aLoseBcks)
    {
        m_WinFishBucks = aWinBcks;
        m_LoseFishBucks = aLoseBcks;
    }

    private void SetPlayerStoryEvent(int winnerId)
    {
        if (winnerId != FightUtil.GetPlayerID()) { return; }

        switch (FightUtil.CoreData.FightIndex)
        {
            case FightIndex.TutorialFight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.WonFirstFight, true);
                break;

            case FightIndex.Area2Fight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.WonSecondFight, true);
                break;

            case FightIndex.Area3Fight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.WonThirdFight, true);
                break;

            case FightIndex.LighthouseFight:
                GameUtil.PlayerState.SetPlayerStoryEvent(PlayerStoryEvent.WonFourthFight, true);
                break;
        }
    }

    private void DistributePlayerRewards(int winnerId)
    {
        if (winnerId == FightUtil.GetPlayer().GetID())
        {
            //Experience Distribution.
            GameUtil.PlayerState.ExperiencePoints += m_WinPoints;
            GameUtil.EventManager.AddEvent(new AddPlayerXpPointsEvent(m_WinPoints));

            GameUtil.PlayerState.SkillPoints += FightUtil.CoreData.WinSkillPoints;

            GameUtil.PlayerState.FishBucks += FightUtil.CoreData.WinFishBucks;
        }
        else
        {
            //Experience Distribution.
            GameUtil.PlayerState.ExperiencePoints += m_LosePoints;
            GameUtil.EventManager.AddEvent(new AddPlayerXpPointsEvent(m_LosePoints));

            GameUtil.PlayerState.SkillPoints += FightUtil.CoreData.LoseSkillPoints;

            GameUtil.PlayerState.FishBucks += FightUtil.CoreData.LoseFishBucks;
        }
    }

    public void UnRegisterToEventSystem() =>
        m_eventManager.UnRegisterSubscribers(HandleRoundBeginEvent, HandleFighterLostEvent);

    public bool IsRoundInProcess() => m_RoundInProcess;
}
}
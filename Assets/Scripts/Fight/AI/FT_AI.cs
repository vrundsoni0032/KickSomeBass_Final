using System.Collections.Generic;
using Core.EventSystem;
using Fight.Events;
using Fight.Fighter;
using UnityEngine;

namespace Fight.AI
{
public class FT_AI : FT_Fighter
{
    [SerializeField] private float m_minDistanceFromPlayer = 1.0f;
    [SerializeField] private float m_maxDistanceFromPlayer = 100.0f;
    public List<string> abilityList;

    private FT_Brain m_brain;
    
    protected override void OnStart()
    {
        m_MaxStamina = 100.0f;
        m_MaxHealth = 100.0f;

        Health = m_MaxHealth;
        Stamina = m_MaxStamina;

        m_brain = GetComponent<FT_Brain>();

        EventManager.RegisterSubscriber(HandleChangeHealthEvent, FT_ChangeHealthEvent.EventID);
        EventManager.RegisterSubscriber(HandlePlayerStateChangeEvent, FT_PlayerStateChangedEvent.EventID);

        EventManager.RegisterSubscriber(StartBrainProcess, FT_RoundBeginEvent.EventID);
        EventManager.RegisterSubscriber(EndBrainProcess, FT_RoundOverEvent.EventID);
        EventManager.RegisterSubscriber(EndBrainProcess, FT_FightOverEvent.EventID);
    }

    private void HandlePlayerStateChangeEvent(KSB_IEvent gameEvent)
    {
        FT_PlayerStateChangedEvent stateChangedEvent = (FT_PlayerStateChangedEvent)gameEvent;

        if(stateChangedEvent.ActionName == "Movement") { return; }
    }


    private void HandleChangeHealthEvent(KSB_IEvent gameEvent)
    {
        FT_ChangeHealthEvent healthChangeEvent = (FT_ChangeHealthEvent)gameEvent;
        if (healthChangeEvent.Fighter.GetID() != GetID()) { return; }

        SetHealth(healthChangeEvent.ChangeAmount);
        
        if (Health <= 0.0f)
        {
            if (m_animationHandler != null)
            {
                m_animationHandler.StartAnimation("idle");
            }
            EventManager.AddEvent(new FT_FighterLostEvent(this));
        }
    }

    private void StartBrainProcess(KSB_IEvent gameEvent) => m_brain.StartProcess();
    private void EndBrainProcess(KSB_IEvent gameEvent) => m_brain.StopProcess();

    public override int GetID() => 1;

    public override void UnRegisterToEventSystem()
    {
        m_brain.StopProcess();

        base.UnRegisterToEventSystem();
        EventManager.UnRegisterSubscribers(
        StartBrainProcess,
        EndBrainProcess,
        HandlePlayerStateChangeEvent,
        HandleChangeHealthEvent);
    }

    public float MinDistanceFromPlayer => m_minDistanceFromPlayer;
    public float MaxDistanceFromPlayer => m_maxDistanceFromPlayer;
}
}
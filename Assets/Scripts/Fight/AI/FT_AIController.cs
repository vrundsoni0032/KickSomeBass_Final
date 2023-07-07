using Core.EventSystem;
using Fight.Events;
using Core.Controller;
using Fight.AbilitySystem;

namespace Fight.AI
{
public class FT_AIController : KSB_IController
{
    private readonly FT_AI m_AI;
    private readonly KSB_IEventManager m_eventManager;
    private string m_lastAction;

    private bool m_bMove;
    private FT_MoveDirection m_moveDirection;

    public FT_AIController(FT_AI AI)
    {
        m_AI = AI;
        m_eventManager = GameUtil.EventManager;
        m_eventManager.RegisterSubscriber(HandleInputEvent, FT_AIInputEvent.EventID);
    }

    public void UnRegisterToEventSystem() => m_eventManager.UnRegisterSubscribers(HandleInputEvent);

    public  void HandleInputEvent(KSB_IEvent gameEvent)
    {
        FT_AIInputEvent inputEvent = (FT_AIInputEvent)gameEvent;

        //YCLogger.Info("AIController", "Performed action - " + inputEvent.Action);

        if (m_lastAction == "AIStaminaGain")
        {
            if (inputEvent.Action != "AIStaminaGain")
            {
                m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(m_AI, false, false, false, false));
                m_eventManager.AddEvent(new FT_AbilityEvent(m_AI, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_AI.GetID()), FT_AbilityState.End));
            }
            else
            {
                return;
            }
        }

        m_lastAction = inputEvent.Action;

        switch (m_lastAction)
        {
            case "AIMoveIn":    m_bMove = true;  m_moveDirection = FT_MoveDirection.Toward; break;
            case "AIMoveAway":  m_bMove = true;  m_moveDirection = FT_MoveDirection.Away;   break;
            case "AIMoveRight": m_bMove = true;  m_moveDirection = FT_MoveDirection.Right;  break;
            case "AIMoveLeft":  m_bMove = true;  m_moveDirection = FT_MoveDirection.Left;   break;
         
            case "AIJump":      m_bMove = false; m_eventManager.AddEvent(new FT_JumpEvent(FightUtil.GetAI())); break;
        
            case "AIDashAway":
                m_bMove = false;
                m_eventManager.AddEvent(new FT_AbilityEvent(m_AI, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Away, m_AI.GetID())));
                break;

            case "AIStaminaGain":
                m_bMove = false;
                m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(m_AI, true, true, true, false));
                m_eventManager.AddEvent(new FT_AbilityEvent(m_AI, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_AI.GetID())));
                break;

            case "AIPunch": case "AIHeadBash": case "AIBootThrow": case "AIStrum": case "AISpin": case "AIDashIn":
                m_bMove = false;
                m_eventManager.AddEvent(new FT_AbilityEvent(m_AI, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_AI.GetID())));
                break;
        }
    }

    public void UpdateMovement()
    {
        if (!m_bMove) { return; }
        m_eventManager.AddEvent(new FT_MoveEvent(m_AI, FightUtil.GetRelativeDirection(m_moveDirection, m_AI.GetID())));
    }
}
}
using Core.Component;
using UnityEngine;
using Core.Controller;
using Core.EventSystem;
using Core.InputSystem;
using Fight.AbilitySystem;
using Fight.Fighter;
using Fight.Events;
using Fight.Player;

namespace Fight.PlayerController
{
public class FT_PlayerController : KSB_IController
{
    private FT_Fighter m_player;
    private KSB_IEventManager m_eventManager;
    private Transform m_cameraTransform;

    public FT_PlayerController(FT_Player player, Transform cameraTransform)
    {
        m_player = player;
        m_eventManager = GameUtil.EventManager;
        m_eventManager.RegisterSubscriber(HandleInputEvent, KSB_InputEvent.EventID);

        m_player = player;
        m_cameraTransform = cameraTransform;
    }

    public void UnRegisterToEventSystem() { m_eventManager.UnRegisterSubscribers(HandleInputEvent); }

    public  void HandleInputEvent(KSB_IEvent gameEvent)
    {
        KSB_InputEvent inputEvent = (KSB_InputEvent)gameEvent;
        
        switch (inputEvent.Action)
        {
            case "Movement":
                    Vector3 axisInput = inputEvent.AxisDirection;
                    m_eventManager.FastForwardEventToNotifySubscribersImmediately(new FT_MoveEvent(m_player, m_cameraTransform.forward * axisInput.z + m_cameraTransform.right * axisInput.x));
                    break;

            case "Jump":
                m_eventManager.FastForwardEventToNotifySubscribersImmediately(new FT_JumpEvent(m_player));
                break;

            case "Dash":
                GameUtil.EventManager.AddEvent(new FT_PlayerStateChangedEvent(inputEvent.Action));
                m_eventManager.AddEvent(new FT_AbilityEvent(m_player, inputEvent.Action, m_player.GetComponent<MovementComponent>().GetMovementDirection(), FT_AbilityState.Begin));
                break;
            case "HeadBash":
            case "SpinThrow":
            case "DynamiteThrow":
            case "Punch":
            case "BootThrow":
                GameUtil.EventManager.AddEvent(new FT_PlayerStateChangedEvent(inputEvent.Action));
                m_eventManager.AddEvent(new FT_AbilityEvent(m_player, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_player.GetID()), FT_AbilityState.Begin));
                break;

                case "StaminaGain":
                GameUtil.EventManager.AddEvent(new FT_PlayerStateChangedEvent(inputEvent.Action));
                if (inputEvent.KeyState == KSB_InputKeyState.KeyPressedOnce)
                {
                    m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(m_player, true, true, false, false));
                    m_eventManager.AddEvent(new FT_AbilityEvent(m_player, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_player.GetID()), FT_AbilityState.Begin));
                }
                else if (inputEvent.KeyState == KSB_InputKeyState.KeyUp)
                {
                    m_eventManager.AddEvent(new FT_FreezeFighterActionEvent(m_player, false, false, false, false));
                    m_eventManager.AddEvent(new FT_AbilityEvent(m_player, inputEvent.Action, FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, m_player.GetID()), FT_AbilityState.End));
                }
                break;

                case "UseItem1": case "UseItem2":case "UseItem3":case "UseItem4":
                int ItemIndex = int.Parse(inputEvent.Action.Substring(7))-1;
                m_eventManager.AddEvent(new FT_UseItemEvent(m_player, ItemIndex));
                break;
        }
    }
}
}
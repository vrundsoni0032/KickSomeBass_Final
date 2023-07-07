using PrivateCore.PersistentController;
using Core.EventSystem;
using Core.InputSystem;
using UnityEngine;

namespace PrivateCore.InputSystem
{
public class KSB_InputManager
{
    private KSB_SO_CoreInput m_coreInput;
    private KSB_PersistentController m_persistentController;

    private bool m_interruptGameplayInput = true;

    public KSB_InputManager(KSB_IEventManager eventManager, KSB_SO_CoreInput coreInput)
    {
        m_coreInput = coreInput;
        m_persistentController = new KSB_PersistentController(eventManager);
        eventManager.RegisterSubscriber(HandleInterruptGameplayInputEvent, KSB_InterruptGameplayInputEvent.EventID);
        eventManager.RegisterSubscriber(HandleMouseStateEvent, KSB_MouseStateEvent.EventID);
    }

    public void UpdateInputDevice(KSB_InputPhase inputPhase)
    {
        m_coreInput.UpdateCoreInput(KSB_InputPhase.Persistent, KSB_InputDeviceType.KeyboardMouse);
        m_coreInput.UpdateCoreInput(KSB_InputPhase.Persistent, KSB_InputDeviceType.Gamepad);

        if(!m_interruptGameplayInput)
        {
            m_coreInput.UpdateCoreInput(inputPhase, KSB_InputDeviceType.KeyboardMouse);
            m_coreInput.UpdateCoreInput(inputPhase, KSB_InputDeviceType.Gamepad);
        }
    }

    public void HandleInterruptGameplayInputEvent(KSB_IEvent gameEvent) =>
        m_interruptGameplayInput = ((KSB_InterruptGameplayInputEvent)gameEvent).InterruptGameplayInput;

    public void HandleMouseStateEvent(KSB_IEvent gameEvent)
    {
        KSB_MouseStateEvent mouseStateEvent = (KSB_MouseStateEvent)gameEvent;
        Cursor.lockState = mouseStateEvent.CursorState;
        Cursor.visible = mouseStateEvent.Visibility;
    }
}
}
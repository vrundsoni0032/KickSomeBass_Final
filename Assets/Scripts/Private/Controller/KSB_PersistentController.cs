using Core;
using Core.EventSystem;
using Core.Controller;
using InteractiveMap.Events;
using UnityEngine;

namespace PrivateCore.PersistentController
{
public class KSB_PersistentController : KSB_IController
{
    private KSB_IEventManager m_eventManager;

    public KSB_PersistentController(KSB_IEventManager eventManager)
    {
        m_eventManager = eventManager;
        m_eventManager.RegisterSubscriber(HandleInputEvent, KSB_InputEvent.EventID); 
    }

    public void UnRegisterToEventSystem() { m_eventManager.UnRegisterSubscribers(HandleInputEvent); }

    public  void HandleInputEvent(KSB_IEvent gameEvent)
    {
        KSB_InputEvent inputEvent = (KSB_InputEvent)gameEvent;

        switch(inputEvent.Action)
        {
        case "Escape":
            //todo: uncomment
            m_eventManager.AddEvent(new KSB_EscapeEvent());
            //IM_Core.isAnyCanvasActive = false;
            //Debug.Log(IM_Core.isAnyCanvasActive);
            break;

        //case "Pause":
        //    m_eventManager.AddEvent(new KSB_PauseEvent());
        //    break;

        //God Mode
        case "SceneChange":
            m_eventManager.AddEvent(new KSB_EndSceneEvent("IM_Scene", GameUtil.MapCore, null));
            break;
        }
    }
}
}
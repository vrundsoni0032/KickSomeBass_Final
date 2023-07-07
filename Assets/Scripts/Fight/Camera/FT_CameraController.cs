using Core;
using Core.CameraSystem;
using Core.Controller;
using Core.EventSystem;
using Fight.Events;
using Fight.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.CameraSystem
{
    public class FT_CameraController : KSB_IController
    {
        KSB_IEventManager m_eventManager;
        FT_Player m_player;
        BaseCamera m_camera;

        // Start is called before the first frame update
        public FT_CameraController(KSB_IEventManager aEventManager, FT_Player aPlayer, GameObject aCamera)
        {
            m_eventManager = aEventManager;
            m_player = aPlayer;
            m_camera = aCamera.GetComponent<BaseCamera>();

            m_eventManager.RegisterSubscriber(HandleInputEvent, KSB_InputEvent.EventID);
        }

        public void HandleInputEvent(KSB_IEvent aEvent)
        {
            KSB_InputEvent inputEvent = (KSB_InputEvent)aEvent;

            switch (inputEvent.Action)
            {
                case "MouseAxis":
                TPSFollowCamera followCamera = (TPSFollowCamera)m_camera;
                if (followCamera)
                {   
                    followCamera.MouseMoved(inputEvent.AxisDirection);
                }
                break;
            }
        }

        public void UnRegisterToEventSystem()
        {
            m_eventManager.UnRegisterSubscriberEvents(HandleInputEvent);
        }
    }
}

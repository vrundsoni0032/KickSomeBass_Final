using GameInterface.Events;
using Core.EventSystem;
using UnityEngine;

namespace GameInterface.MainMenu
{
public class GI_MainMenuManager
{
    private GameObject m_currentUIObject;

    public GI_MainMenuManager(KSB_IEventManager eventManager)
    {
        eventManager.RegisterSubscriber(HandleEndTitleScreenEvent , GI_EndTitleScreenEvent.EventID);
        eventManager.RegisterSubscriber(HandleInstantiateUIPrefabEvent, GI_InstantiateUIPrefabEvent.EventID);
        eventManager.RegisterSubscriber(HandleDestroyCurrentUIPrefabEvent, GI_DestroyCurrentUIPrefabEvent.EventID);
    }

    private void HandleEndTitleScreenEvent(KSB_IEvent gameEvent)
    {
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        m_currentUIObject = GameUtil.InstantiatePrefabInActiveScene("GameInterface/MainMenu/GI_Prb_MainMenu");
        //GI_TitleScreen titleScreen = TitleScreen.GetComponent<GI_TitleScreen>();
    }

    private void HandleInstantiateUIPrefabEvent(KSB_IEvent gameEvent)
    {
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));

        m_currentUIObject = GameUtil.InstantiatePrefabInActiveScene(((GI_InstantiateUIPrefabEvent)gameEvent).PrefabToInstantiateLiteral);
    }

    private void HandleDestroyCurrentUIPrefabEvent(KSB_IEvent gameEvent)
    {
        GameObject.Destroy(m_currentUIObject);
    }
}
}
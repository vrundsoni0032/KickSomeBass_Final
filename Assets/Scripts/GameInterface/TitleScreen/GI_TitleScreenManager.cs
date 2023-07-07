using UnityEngine;
using Core.EventSystem;
using GameInterface.Events;

namespace GameInterface.TitleScreen
{
public class GI_TitleScreenManager
{
    GI_TitleScreen m_titleScreen;
    public GI_TitleScreenManager(KSB_IEventManager eventManager)
    {
        eventManager.RegisterSubscriber(HandleTitleScreenEndEvent , GI_EndTitleScreenEvent.EventID);
        GameObject TitleScreen = GameUtil.InstantiatePrefabInActiveScene("GameInterface/TitleScreen/GI_Prb_TitleScreen");
        m_titleScreen = TitleScreen.GetComponent<GI_TitleScreen>();
    }

    private void HandleTitleScreenEndEvent(KSB_IEvent gameEvent)
    {
        GameUtil.EventManager.AddEvent(new KSB_ClearTimerEvent());
        Object.Destroy(m_titleScreen.gameObject);
    }
}
}
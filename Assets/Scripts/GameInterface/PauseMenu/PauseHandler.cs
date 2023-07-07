using UnityEngine;
using Core.EventSystem;
using GameInterface.PauseMenu;

namespace GameInterface.MainMenu
{
public class PauseHandler
{
    [Header("Pause SCREEN PREFAB")]
    [SerializeField] public GI_PauseMenu m_pauseMenu;

    public PauseHandler(GI_PauseMenu pauseMenu, KSB_IEventManager eventManager)
    {
        m_pauseMenu = pauseMenu;
        eventManager.RegisterSubscriber(HandlePauseEvent, KSB_EscapeEvent.EventID);
    }

    public void HandlePauseEvent(KSB_IEvent gameEvent)
    {
        if (Time.timeScale >= 1.0f && !GameUtil.GetCurrentCore().IsAnyCanvasActive())
        {
            Time.timeScale = 0;
            m_pauseMenu.TogglePauseMenu(true);
            GameUtil.GetCurrentCore().SetCanvasActive(true);
        }
        else if (Time.timeScale <= 0.0f)
        {
            Time.timeScale = 1;
            m_pauseMenu.TogglePauseMenu(false);
            GameUtil.GetCurrentCore().SetCanvasActive(false);
        }
    }
}
}
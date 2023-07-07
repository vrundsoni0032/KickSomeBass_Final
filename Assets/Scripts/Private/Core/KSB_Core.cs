using UnityEngine;
using Core;
using Core.AudioSystem;
using GameInterface.MainMenu;
using GameInterface.PauseMenu;
using PrivateCore.SceneSystem;
using PrivateCore.CoreHelper;
using PrivateCore.InputSystem;

namespace PrivateCore
{
public class KSB_Core : MonoBehaviour
{
    [SerializeField] private string m_DefaultLevelToLoad = "IM_Scene";
    [SerializeField] private bool m_showTitleScreens = false;
    [SerializeField] private bool m_showMainMenu = false;

    [SerializeField] private PlayerState m_PlayerState;
    [SerializeField] private KSB_SO_CoreInput m_coreInput;
    [SerializeField] private KSB_SO_SoundManager m_soundManager;

    [SerializeField] private GI_PauseMenu m_PauseMenu;
    [SerializeField] private PauseHandler m_pauseHandler;

    private TriggerTimer m_triggerTimer;
    private KSB_EventManager m_eventManager;

    private IM_Core m_MapCore;
    private FT_Core m_FightCore;

    private KSB_SceneManager m_sceneManager;
    private KSB_InputManager m_inputManager;

    //private EscapeHandler m_escapeHandler; //Temp

    private void Start()
    {
        m_eventManager = new KSB_EventManager();
        m_triggerTimer = new TriggerTimer(m_eventManager);
        m_inputManager = new KSB_InputManager(m_eventManager, m_coreInput);
        m_sceneManager = new KSB_SceneManager(m_eventManager);
        m_soundManager.Init(gameObject.GetComponents<AudioSource>()[0], gameObject.GetComponents<AudioSource>()[1]);

        m_MapCore = new IM_Core();
        m_FightCore = new FT_Core();

        GameUtil.Initialize(m_MapCore, m_FightCore, m_eventManager, m_soundManager, m_triggerTimer, m_sceneManager.GetCurrentCore, m_PlayerState);
        m_pauseHandler = new PauseHandler(m_PauseMenu, m_eventManager);

        m_sceneManager.Init(m_DefaultLevelToLoad, m_showTitleScreens, m_showMainMenu);
    }

    private void Update()
    {
        m_eventManager.NotifySubscribers(10);
        m_triggerTimer.UpdateTimers();
        m_sceneManager.Update();

        if (m_sceneManager.IsSceneReadyToUpdate())
        {
            m_sceneManager.GetCurrentCore().UpdateSceneDataAssets();
            m_inputManager.UpdateInputDevice(KSB_CoreHelper.GetInputPhase(m_sceneManager.GetCurrentCore().GetCoreType()));
        }
    }
}
}
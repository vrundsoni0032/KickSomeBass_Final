using Core;
using PrivateCore.CoreHelper;
using Core.EventSystem;
using GameInterface.TitleScreen;
using GameInterface.MainMenu;
using GameInterface.LoadScreen;
using UnityEngine;


namespace PrivateCore.SceneSystem
{
public class KSB_SceneManager
{
    private KSB_IEventManager m_eventManager;
    private GI_LoadScreenManager m_LoadScreenManager;
    private KSB_ICore m_currentCore;
    private KSB_CoreData m_currentCoreData;
    private bool m_IsCurrentSceneInitialized = false;

    public KSB_SceneManager(KSB_IEventManager eventManager)
    {
        m_eventManager = eventManager;
        m_eventManager.RegisterSubscriber(HandleFirstSceneToLoadEvent, KSB_FirstSceneToLoadEvent.EventID);
        m_eventManager.RegisterSubscriber(HandleEndSceneEvent, KSB_EndSceneEvent.EventID);
        m_eventManager.RegisterSubscriber(HandleGameQuitEvent, KSB_GameQuitEvent.EventID);
        
    }

    public void Init(string defaultLevelToLoad, bool bShowTitleScreen, bool bShowMainMenu)
    {
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
        if(bShowTitleScreen) { GI_TitleScreenManager titleScreenManager = new GI_TitleScreenManager(m_eventManager);}
        if(bShowMainMenu) { GI_MainMenuManager mainMenuManager = new GI_MainMenuManager(m_eventManager);}
        else 
        {
            KSB_CoreType coreType = KSB_CoreHelper.GetCoreType(defaultLevelToLoad);
            HandleFirstSceneToLoadEvent(new KSB_FirstSceneToLoadEvent(defaultLevelToLoad, GameUtil.GetCore(coreType), false)); 
        }
    }

    public void Update()
    {
        if (KSB_SceneLoadHelper.IsNewSceneLoading())
        {
            m_LoadScreenManager.Update();

            if(m_LoadScreenManager.IsReadyToDestroyLoadScreen() && KSB_SceneLoadHelper.SetActiveScene())
            {
                m_IsCurrentSceneInitialized = m_currentCore.LateInitialize();
                m_LoadScreenManager.CleanLoadScreen();
                m_eventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
                //KSB_SceneLoadHelper.UnLoadScene();
            }
        }
    }

    public void HandleFirstSceneToLoadEvent(KSB_IEvent gameEvent)
    {
        m_eventManager.AddEvent(new KSB_MouseStateEvent(false));

        KSB_FirstSceneToLoadEvent firstSceneToLoadEvent = (KSB_FirstSceneToLoadEvent)gameEvent;
        //if(firstSceneToLoadEvent.LoadNewGame)
        { LoadSceneWithLoadScreen(firstSceneToLoadEvent.SceneToLoadName, firstSceneToLoadEvent.SceneToLoadCore); }
        //else if(firstSceneToLoadEvent.LoadFromSaveFile)

    }

    public void HandleGameQuitEvent(KSB_IEvent gameEvent)
    {
        if(m_currentCore != null) { UnloadCurrentScene(); }
        UnityEngine.Application.Quit();
    }

    private void HandleEndSceneEvent(KSB_IEvent gameEvent)
    {
        KSB_EndSceneEvent endSceneEvent = (KSB_EndSceneEvent)gameEvent;

        m_eventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
        m_IsCurrentSceneInitialized = false;
        m_currentCoreData = endSceneEvent.CoreData;
        UnloadCurrentScene();
        m_eventManager.AddEvent(new KSB_ClearTimerEvent());
        LoadSceneWithLoadScreen(endSceneEvent.SceneToLoadName, endSceneEvent.SceneToLoadCore);
    }

    public bool LoadSceneWithLoadScreen(string SceneToLoad, KSB_ICore SceneToLoadCore)
    {
        m_LoadScreenManager = new GI_LoadScreenManager(KSB_CoreHelper.GetLoadScreenType(SceneToLoadCore.GetCoreType()),
                              KSB_SceneLoadHelper.GetSceneLoadingProgress, KSB_SceneLoadHelper.IsSceneLoaded);

        m_LoadScreenManager.InstantiateAppriopriateLoadScreen();

        m_currentCore = SceneToLoadCore;
        //YCLogger.Debug("Current Core - ", m_currentCore.GetCoreType(), " , SceneToLoadCore - ", SceneToLoadCore.GetCoreType());
        KSB_SceneLoadHelper.LoadSceneAsync(SceneToLoad);

        bool doneInitialize = SceneToLoadCore.InitializeSceneDataAssets(m_currentCoreData);

        YCLogger.Assert(SceneToLoad.ToString(), doneInitialize, "Failed to Initialize the scene data.");

        m_eventManager.AddEvent(new KSB_MouseStateEvent(false));
        return true;
    }

    public bool UnloadCurrentScene()
    {
        bool doneCleanup = m_currentCore.CleanUpSceneDataAssets();
        YCLogger.Assert(m_currentCore.ToString(), doneCleanup, "Failed to clean up the scene data.");

        KSB_SceneLoadHelper.UnLoadActiveScene();
        return true;
    }

    public bool IsSceneReadyToUpdate() { return m_IsCurrentSceneInitialized; }
    public KSB_ICore GetCurrentCore() { return m_currentCore; }
}
}
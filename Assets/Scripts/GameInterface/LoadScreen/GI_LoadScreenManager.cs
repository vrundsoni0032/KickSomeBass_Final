using UnityEngine;

namespace GameInterface.LoadScreen
{
public class GI_LoadScreenManager
{
    private GI_LoadScreen m_loadScreen;

    private GI_LoadScreenType m_loadScreenType;
    private GetSceneProgress m_sceneProgressDelegate;
    private IsSceneLoaded m_isSceneLoadedDelegate;

    private bool m_IsReadyToDestroyLoadScreen = false;

    public GI_LoadScreenManager(GI_LoadScreenType loadScreenType, GetSceneProgress sceneProgressFunctionPointer, IsSceneLoaded isSceneLoadedFunctionPointer)
    {
        m_loadScreenType = loadScreenType;
        m_sceneProgressDelegate = sceneProgressFunctionPointer;
        m_isSceneLoadedDelegate = isSceneLoadedFunctionPointer;
    }

    public GI_LoadScreen InstantiateAppriopriateLoadScreen()
    {
        m_IsReadyToDestroyLoadScreen = false;

        GameObject LoadScreen = GameUtil.InstantiatePrefabInActiveScene("GameInterface/LoadScreen/GI_Prb_LoadScreen");
        m_loadScreen = LoadScreen.GetComponent<GI_LoadScreen>();

        m_loadScreen.EnableStaticCanvas(true, m_loadScreenType);
        m_loadScreen.EnableDynamicCanvas(true, 0);

        return m_loadScreen;
    }

    public bool IsReadyToDestroyLoadScreen() { return m_IsReadyToDestroyLoadScreen; }

    public void CleanLoadScreen() { Object.Destroy(m_loadScreen.gameObject); }

    public void Update()
    {
        if(!m_isSceneLoadedDelegate.Invoke()) { m_loadScreen.UpdateLoadingBar(m_sceneProgressDelegate.Invoke()); }
        else 
        {
            if(m_loadScreen.AutoSkipOnLoad()) { m_IsReadyToDestroyLoadScreen = true; }
            else //Only show text to skip when scene is ready & wait for input.
            {
                m_loadScreen.EnableInstructionTextToSkip(true);  
                if(Input.anyKeyDown) { m_IsReadyToDestroyLoadScreen = true; } 
            } 
        }
    }
}
}
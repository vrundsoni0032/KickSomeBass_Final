using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrivateCore.SceneSystem
{
public static class KSB_SceneLoadHelper
{
    private static string m_ActiveSceneName;
    private static bool m_IsSceneLoading = false;
    private static AsyncOperation m_SceneToLoadProgress;

    public static bool LoadSceneAsync(string sceneToLoad)
    {
        m_ActiveSceneName = sceneToLoad;
        m_SceneToLoadProgress = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        m_IsSceneLoading = true;

        YCLogger.Info("SceneLoader", "Loading " + sceneToLoad + " scene asynchronously.");

        return true;
    }
    public static bool UnLoadActiveScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(m_ActiveSceneName));
        YCLogger.Info("SceneLoader", m_ActiveSceneName + " is unloaded.");
        return true;
    }

    public static bool UnLoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
        YCLogger.Info("SceneLoader", sceneName + " is unloaded.");
        return true;
    }

    public static bool SetActiveScene()
    {
        if(!m_SceneToLoadProgress.isDone) { return false; }
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_ActiveSceneName));
            YCLogger.Info("SceneLoader", m_ActiveSceneName + " is load and is active scene now.");

            m_IsSceneLoading = false;
        }
        return true;
    }
    
    public static bool IsSceneLoaded() { return m_SceneToLoadProgress.isDone; }
    public static bool IsNewSceneLoading() { return m_IsSceneLoading; }
    public static float GetSceneLoadingProgress() { return Mathf.Clamp01(m_SceneToLoadProgress.progress/0.9f); }
}
}
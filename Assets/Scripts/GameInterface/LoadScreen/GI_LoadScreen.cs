using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameInterface.LoadScreen
{
public delegate float GetSceneProgress();
public delegate bool IsSceneLoaded();

public enum GI_LoadScreenType
{
    NullScreen,
    FightLoadScreen,
    MapLoadScreen
}

[System.Serializable] public struct LoadScreenDictionary { public GI_LoadScreenType ScreenType; public GI_SO_LoadScreen LoadScreen; }

public class GI_LoadScreen : MonoBehaviour
{
    [SerializeField] private List<LoadScreenDictionary> m_LoadScreens;

    [SerializeField] private TMPro.TextMeshProUGUI m_hintText;
    [SerializeField] private Image m_loadScreenImage;
    [SerializeField] private Slider m_progressBar;
    [SerializeField] private TMPro.TextMeshProUGUI m_instructionText;
    [SerializeField] private Canvas m_staticCanvas;
    [SerializeField] private Canvas m_dynamicCanvas;

    private bool m_AutoSkipOnLoad = false;

    
    public void UpdateLoadingBar(float loadProgress) { m_progressBar.value = loadProgress; }

    public void EnableStaticCanvas(bool enable, GI_LoadScreenType screenType) 
    {
        if(enable)
        {
            foreach(LoadScreenDictionary loadScreen in m_LoadScreens)
            {
                if(loadScreen.ScreenType == screenType) { HandleStaticCanvasAssetLoad(loadScreen.LoadScreen); }
            }
        }

        m_staticCanvas.enabled = enable; 
    }

    public void EnableDynamicCanvas(bool enable, float progressValue)
    {
        m_dynamicCanvas.enabled = enable;
        m_progressBar.value = progressValue;
    }

    public void EnableInstructionTextToSkip(bool enable = true) { m_instructionText.gameObject.SetActive(enable); }

    private void HandleStaticCanvasAssetLoad(GI_SO_LoadScreen loadScreen) //Doesn't handle the skipable screen yet.
    {
        m_AutoSkipOnLoad = loadScreen.AutoSkipOnLoad();

        if(loadScreen.IsLoadScreenRandomize()) { m_loadScreenImage.sprite = loadScreen.GetRandomLoadScreen(); }
        else { m_loadScreenImage.sprite = loadScreen.GetLoadScreen(0); }

        if(loadScreen.IsTipEnable())
        {
            if(loadScreen.IsTipRandomize()) { m_hintText.text = loadScreen.GetRandomHint(); }
            else { m_hintText.text = loadScreen.GetHint(0); }
        }
    }

    public bool AutoSkipOnLoad() { return m_AutoSkipOnLoad; }
}
}
using System.Collections.Generic;
using Fight.Events;
using UnityEngine;
using GameInterface.Events;
using UnityEngine.UI;

public class GI_VideoOptions : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void FullScreen(bool isfullscreen)
    {
        Screen.fullScreen = isfullscreen;

        Debug.Log(isfullscreen);
    }

    public void OnClick_Back()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuOptions"));
    }
}

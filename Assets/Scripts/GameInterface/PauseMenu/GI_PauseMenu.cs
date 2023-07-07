using Core;
using UnityEngine;
using Core.EventSystem;

namespace GameInterface.PauseMenu
{
public class GI_PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject IM_PauseLayer;
    [SerializeField] private GameObject IM_OptionsLayer;

    [SerializeField] private GameObject FT_PauseLayer;
    [SerializeField] private GameObject FT_OptionsLayer;

    public void TogglePauseMenu(bool enable)
    {
        gameObject.SetActive(enable);
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(enable));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(enable));

        if (GameUtil.GetCurrentCore().GetCoreType() == KSB_CoreType.InteractiveMap)
        {
            IM_PauseLayer.SetActive(true);
            IM_OptionsLayer.SetActive(false);
        }
        else // is equal to KSB_CoreType.Fight
        {
            FT_PauseLayer.SetActive(true);
            FT_OptionsLayer.SetActive(false);
        }
    }

    //Button Functions
    public void OnClick_Resume()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");

        GameUtil.EventManager.AddEvent(new KSB_EscapeEvent());

        if (GameUtil.GetCurrentCore().GetCoreType() == KSB_CoreType.InteractiveMap)
        {
            IM_PauseLayer.SetActive(false);
            IM_OptionsLayer.SetActive(false);
        }
        else // is equal to KSB_CoreType.Fight
        {
            FT_PauseLayer.SetActive(false);
            FT_OptionsLayer.SetActive(false);
        }
    }

    public void OnClick_Options()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));

        //PauseLayer.SetActive(false);
        if (GameUtil.GetCurrentCore().GetCoreType() == KSB_CoreType.InteractiveMap)
        {
            IM_PauseLayer.SetActive(false);
            IM_OptionsLayer.SetActive(true);
        }
        else // is equal to KSB_CoreType.Fight
        {
            FT_PauseLayer.SetActive(false);
            FT_OptionsLayer.SetActive(true);
        }
    }

    public void OnClick_Back()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");

        if (GameUtil.GetCurrentCore().GetCoreType() == KSB_CoreType.InteractiveMap)
        {
            IM_PauseLayer.SetActive(true);
            IM_OptionsLayer.SetActive(false);
        }
        else // is equal to KSB_CoreType.Fight
        {
            FT_PauseLayer.SetActive(true);
            FT_OptionsLayer.SetActive(false);
        }
    }

    public void OnClick_Quit()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new KSB_GameQuitEvent());
    }

    public void OnClick_ReturnToMap()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        Time.timeScale = 1;
        FT_PauseLayer.SetActive(false);
        FT_OptionsLayer.SetActive(false);
        GameUtil.EventManager.AddEvent(new KSB_EndSceneEvent("IM_Scene", GameUtil.MapCore, null));
    }
}
}
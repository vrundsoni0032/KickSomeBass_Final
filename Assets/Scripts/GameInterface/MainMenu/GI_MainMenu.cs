using UnityEngine;
using GameInterface.Events;
using Core.EventSystem;

namespace GameInterface.MainMenu
{
public class GI_MainMenu : MonoBehaviour
{
    public void OnClick_LoadGame()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new KSB_FirstSceneToLoadEvent("IM_Scene", GameUtil.MapCore, false));
    }

    public void OnClick_Play()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new KSB_FirstSceneToLoadEvent("IM_Scene", GameUtil.MapCore, true));
    }

    public void OnClick_Options()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuOptions"));
    }

    public void OnClick_Quit()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new KSB_GameQuitEvent());
    }

    public void OnClick_Back()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/MainMenu/GI_Prb_MainMenu"));
    }
}
}
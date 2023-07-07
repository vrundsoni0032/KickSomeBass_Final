using UnityEngine;
using GameInterface.Events;

namespace GameInterface.MainMenu
{
public class GI_MainMenuOptions : MonoBehaviour
{
    public void OnClick_Gameplay()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuGameplayOptions"));
    }

    public void OnClick_Controls()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuControlsOptions"));
    }

    public void OnClick_Video()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuVideoOptions"));
    }

    public void OnClick_Audio()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuAudioOptions"));
    }

    public void OnClick_Back()
    {
            GameUtil.SoundManager.PlaySound("ButtonPop");
            GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/MainMenu/GI_Prb_MainMenu"));
    }
}
}
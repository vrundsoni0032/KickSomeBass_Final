using UnityEngine;
using GameInterface.Events;
using UnityEngine.UI;

public class GI_ControlOption : MonoBehaviour
{
    public void OnClick_Back()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuOptions"));
    }
}
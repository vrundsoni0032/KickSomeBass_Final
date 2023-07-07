using UnityEngine;
using GameInterface.Events;
using UnityEngine.UI;

public class GI_AudioOptions : MonoBehaviour
{
    private Slider[] m_VolumeSliders;

    public void Awake()
    {
        m_VolumeSliders = gameObject.GetComponentsInChildren<Slider>();
    }
    public void Start()
    {      
        m_VolumeSliders[0].value = GameUtil.SoundManager.MasterVolume;
        m_VolumeSliders[1].value = GameUtil.SoundManager.MusicVolume;
        m_VolumeSliders[2].value = GameUtil.SoundManager.SfxVolume;
    }
    public void SetMasterVolume(float Value)
    {
        GameUtil.SoundManager.SetMasterVolume(Value);
    }
    public void SetMusicVolume(float Value)
    {
        GameUtil.SoundManager.SetMusicVolume(Value);
    }
    public void SetEffectVolume(float Value)
    {
        GameUtil.SoundManager.SetSfxVolume(Value);
    }

    public void OnClick_Back()
    {
        GameUtil.SoundManager.PlaySound("ButtonPop");
        GameUtil.EventManager.AddEvent(new GI_DestroyCurrentUIPrefabEvent());
        GameUtil.EventManager.AddEvent(new GI_InstantiateUIPrefabEvent("GameInterface/Options/GI_Prb_MainMenuOptions"));
    }
}

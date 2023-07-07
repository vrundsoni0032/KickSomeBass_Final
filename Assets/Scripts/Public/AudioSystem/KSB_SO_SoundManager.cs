using System.Collections.Generic;
using UnityEngine;

namespace Core.AudioSystem
{
[CreateAssetMenu(fileName = "SoundManager", menuName = "KSBUtilities/Sound/SoundManager")]
public class KSB_SO_SoundManager : ScriptableObject
{
private AudioSource MusicSource, EffectSource;
public KSB_Sound[] m_Sounds;

private List<string> m_SfxQue;
private List<string> m_MusicQue;

[Range(0f, 1.0f)] public float MasterVolume = 1.0f;
[Range(0f, 1.0f)] public float SfxVolume = 1.0f;
[Range(0f, 1.0f)] public float MusicVolume = 0.5f;

public bool MusicToggle = false;
public bool SfxToggle = false;

public void Init(AudioSource musicSource, AudioSource effectSource)
{            
    MusicSource = musicSource;
    EffectSource = effectSource;

    foreach (KSB_Sound sound in m_Sounds)
    {
            AudioClip c = sound.Clip;
            sound.Length = c.length;
    }
}

private void Update() 
{
        
}
public void AddNextMusic(string Clip_Name) 
{           
    if (GetSound(Clip_Name) != null)
    {
        m_MusicQue.Add(Clip_Name);
    }
}
public void AddNextSfx(string Clip_Name)
{
    if (GetSound(Clip_Name) != null)
    {
        m_SfxQue.Add(Clip_Name);
    }
}

public void PlaySound(string name)
{
    KSB_Sound s = GetSound(name);
    if (s.isMusic)
    {
        if (!MusicSource.isPlaying)
        {
            MusicSource.clip = s.Clip;
            MusicSource.volume = s.Volume * MusicVolume * MasterVolume;
            MusicSource.pitch = s.Pitch;
            MusicSource.priority = s.Priority;
            MusicSource.loop = s.isLoop;
            MusicSource.mute = s.isMute;
            MusicSource.Play();
        }
    }
    else
    {
        EffectSource.clip = s.Clip;
        EffectSource.volume = s.Volume * SfxVolume * MasterVolume;
        EffectSource.pitch = s.Pitch;
        EffectSource.priority = s.Priority;
        EffectSource.loop = s.isLoop;
        EffectSource.mute = s.isMute;
        EffectSource.Play();
    }
}
public void PlaySoundAtPosition(string name, Vector3 position)
{
    KSB_Sound s = GetSound(name);
    
    YCLogger.Assert("SoundManager", !s.isMusic, "Sound" + name + "is Music and can not be played in 3D space");
    
    GameObject sound3D = new GameObject("3Dsound");
    sound3D.transform.position = position;
    AudioSource audioSource3D = sound3D.AddComponent<AudioSource>();

    audioSource3D.clip = s.Clip;
    audioSource3D.volume = s.Volume * SfxVolume * MasterVolume;
    audioSource3D.pitch = s.Pitch;
    audioSource3D.priority = s.Priority;
    audioSource3D.loop = s.isLoop;
    audioSource3D.mute = s.isMute;
    audioSource3D.PlayOneShot(audioSource3D.clip);

}


public KSB_Sound GetSound(string name) { return System.Array.Find(m_Sounds, sound => sound.Name == name); }
public void ToggleSound(string name) { GetSound(name).isMute = !GetSound(name).isMute; }
public void ToggleSfx()
{
    SfxToggle = !SfxToggle;
    EffectSource.mute = SfxToggle;
}

public void ToggleMusic()
{
    MusicToggle = !MusicToggle;
    MusicSource.mute = MusicToggle;
}

public KSB_Sound GetCurrentMusic() { return System.Array.Find(m_Sounds, sound => sound.Clip == MusicSource.clip); }

public void SetMasterVolume(float value) { MasterVolume = value; }
public void SetMusicVolume(float value) { MusicVolume = value; }
public void SetSfxVolume(float value) { SfxVolume = value; }

public void SetPriority(string name, int value) { GetSound(name).Priority = value; }
public void SetVolume(string name, float value) { GetSound(name).Volume = value; }
public void SetPitch(string name, float value) { GetSound(name).Pitch = value; }
public void SetLooping(string name, bool value) { GetSound(name).isLoop = value; }
public void SetMute(string name, bool value) { GetSound(name).isMute = value; }

public void EndMusic() => MusicSource.Stop();
public void EndEffect() => EffectSource.Stop();
}
}
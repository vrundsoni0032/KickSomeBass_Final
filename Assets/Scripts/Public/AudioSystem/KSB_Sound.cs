using UnityEngine;

namespace Core.AudioSystem
{
[System.Serializable] public class KSB_Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0.0f, 1.0f)] public float Volume;

    [Range(0.1f, 4.0f)] public float Pitch;

    public float Length;

    public bool isLoop;
    public bool isMusic;
    public bool isMute;
    public int Priority;
}
}
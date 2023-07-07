using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_SetMusicVolume : MonoBehaviour
{
    [SerializeField] private float Value;
    //set the volume of the music
    private void Start() => GameUtil.SoundManager.SetMusicVolume(Value);
}
}
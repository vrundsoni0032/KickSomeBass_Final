using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_SetSfxVolume : MonoBehaviour
{
    [SerializeField] private float Value;
    //set the volume of the sfx
    void Start() => GameUtil.SoundManager.SetSfxVolume(Value);
}
}
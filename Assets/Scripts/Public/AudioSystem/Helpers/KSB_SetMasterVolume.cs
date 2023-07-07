using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_SetMasterVolume : MonoBehaviour
{
    //set the master volume
    [SerializeField] private float Value;
    private void Start() => GameUtil.SoundManager.SetMasterVolume(Value);
}
}
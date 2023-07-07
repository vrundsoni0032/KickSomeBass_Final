using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_Play3DSoundOnStart : MonoBehaviour
{
    [SerializeField] private string SoundName;
    //play a sound at a certain position
    private void Start() => GameUtil.SoundManager.PlaySoundAtPosition(SoundName, gameObject.transform.position);
}
}
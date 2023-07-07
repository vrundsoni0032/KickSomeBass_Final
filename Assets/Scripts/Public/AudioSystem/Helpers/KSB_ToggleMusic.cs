using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_ToggleMusic : MonoBehaviour
{
    //toggle on or off the music
    public void Toggle() => GameUtil.SoundManager.ToggleMusic();
}
}
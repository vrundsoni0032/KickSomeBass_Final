using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_ToggleSfx : MonoBehaviour
{
    //toggle on or off the sfx
    public void Toggle() => GameUtil.SoundManager.ToggleSfx();
}
}
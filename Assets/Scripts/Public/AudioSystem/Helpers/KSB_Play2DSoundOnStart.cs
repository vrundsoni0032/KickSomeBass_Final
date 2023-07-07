using UnityEngine;

namespace Core.AudioSystem
{
public class KSB_Play2DSoundOnStart : MonoBehaviour
{
    
    [SerializeField] private string SoundName;
    public bool EndPreiviousClip = false;

    private void Start()
    { 
        //if you want the previous clip to end, stop it
        if (EndPreiviousClip)
        {
            if(GameUtil.SoundManager.GetCurrentMusic() != null) 
                { 
                    if (GameUtil.SoundManager.GetCurrentMusic().isMusic) { GameUtil.SoundManager.EndMusic(); }
                    else { GameUtil.SoundManager.EndEffect(); }
                }
        }
        //play new sound
        GameUtil.SoundManager.PlaySound(SoundName);
    }
}
}
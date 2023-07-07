using Core.CameraSystem;
using UnityEngine;

namespace CinematicSystem
{
public class CutsceneManager
{
    [SerializeField] CinematicClip m_currentClip;

    public CutsceneManager()
    {
        CinematicShotHelper.Inititalize();
    }

    public bool Update()    //Returns true when executing a cinematic shot
    {
        if (m_currentClip)
        {
            bool isClipPlaying = PlayCinematicClip(m_currentClip);
            if (!isClipPlaying)
            {
                m_currentClip = null;
                return false;
            }
            return true;
        }
        return false;
    }

    bool PlayCinematicClip(CinematicClip aClip)
    {
        return aClip.Play();
    }

    public void StartCutsceneClip(CinematicClip aClip)
    {
     m_currentClip = aClip;
    }
}
}
using UnityEngine;

namespace CinematicSystem
{
[CreateAssetMenu(fileName = "FT_NewCinematicClip", menuName = "KSBUtilities/Fight/Cinematics/Create new Cinematic Clip")]
[System.Serializable]
public class CinematicClip : ScriptableObject
{
    private int currentShotIndex=0;
    public CinematicShot[] m_cinematicShotSequence;

    public void Initialize()
    {
        currentShotIndex = 0;
        if (m_cinematicShotSequence != null && m_cinematicShotSequence.Length > 0)
            m_cinematicShotSequence[currentShotIndex].Initialize();
    }

    public bool Play()
    {
     YCLogger.Debug("CinematicClip", "Current Shot Index : " + currentShotIndex);
     if (m_cinematicShotSequence == null) return false;

     bool currentShotStillPlaying = m_cinematicShotSequence[currentShotIndex].Animate();

    //If Shot's Animate returns false it means shot has ended 
    if (!currentShotStillPlaying)
    {
        //Switch to next shot
        if (m_cinematicShotSequence.Length - 1 > currentShotIndex)
        {
            currentShotIndex++;
            m_cinematicShotSequence[currentShotIndex].Initialize();
        }
        else
        {
            currentShotIndex = 0;
            return false;
        }
    }

        return true;
    }
            
    public void SetCinematicCamera(GameObject aCamera)
    {
        foreach (CinematicShot shot in m_cinematicShotSequence)
        {
            shot.SetCamera(aCamera);
        }
    }

    public void SetTarget(GameObject aTarget)
    {
        foreach (CinematicShot shot in m_cinematicShotSequence)
        {
            shot.SetTarget(aTarget);
        }
    }
    
}
}
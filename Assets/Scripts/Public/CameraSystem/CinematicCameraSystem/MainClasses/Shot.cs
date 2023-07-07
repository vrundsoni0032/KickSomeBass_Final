using Events;
using UnityEngine;

namespace CinematicSystem
{
[System.Serializable]
public class CinematicShot : CinematicShotBase
{
    Vector3 m_cameraDirection;
    Vector3 m_movementDirection;

    float timer;

    public void Initialize()
    {
        //Add transition to move to the inital position and rotation if transition null snap it to initial postio using event FT_InitializeCinematicCamera
    
        Vector3 localSpaceDir = Quaternion.Inverse(m_target.transform.rotation) * CinematicShotHelper.GetShotHorizontalDirection(m_horizontalOrientation);

        Vector3 initialPosition = m_target.transform.position + (localSpaceDir * CinematicShotHelper.GetShotDistance(m_shotDistanceType));
                initialPosition.y += 2.5f* CinematicShotHelper.GetShotVerticalAmount(m_verticalOrientation);
        m_cameraDirection = m_target.transform.position - initialPosition;

        timer= duration;

        GameUtil.EventManager.AddEvent(new InitializeCinematicCamera(initialPosition, m_cameraDirection));

        NextMovementStep();
    }

    public bool Animate()
    {
        NextMovementStep();

        timer -= Time.deltaTime;

        if (timer <= 0)
            return false;

        return true;
    }

    void NextMovementStep()
    {
        m_movementDirection =  Quaternion.Inverse(m_target.transform.rotation) * CinematicShotHelper.GetShotMotionDirection(m_shotMotion);
        BrodcastEventToUpdateCamera(m_movementDirection, m_cameraDirection);
    }

    void BrodcastEventToUpdateCamera(Vector3 aPosition,Vector3 aLookDirection)
    {
        GameUtil.EventManager.AddEvent(new UpdateCinematicCamera(aPosition, aLookDirection));
    }
}
}
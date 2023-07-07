using UnityEngine;

namespace CinematicSystem
{
public class CinematicShotBase
{
    [SerializeField] protected float duration;
    
    protected GameObject m_target;
    protected GameObject m_camera;
    [SerializeField] protected ShotDistance m_shotDistanceType;
    [SerializeField] protected VerticalOrientation m_verticalOrientation;
    [SerializeField] protected HorizontalOrientation m_horizontalOrientation;
    [SerializeField] protected ShotMotion m_shotMotion;
    [SerializeField] protected float m_motionSpeed;

    public void SetCamera(GameObject aCamera) {m_camera=aCamera; }
    public void SetTarget(GameObject aTarget) { m_target = aTarget; }
}
}
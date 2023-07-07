using UnityEngine;

namespace Core.CameraSystem
{
public class TPSFollowCamera : BaseCamera
{
    GameObject m_primaryTarget;

    [SerializeField] LayerMask m_obstacleCheckMask;

    [SerializeField] float m_distance;
    [SerializeField] float m_offsetVertical;
    [SerializeField] float m_offsetHorizontal;

    [SerializeField] float m_followSpeed;
    [SerializeField] float m_lookSensitivity;

    [SerializeField]Vector3 m_currentRotation;
    Vector3 dampVelocity = new Vector3();

    bool m_isFocusingOnPOI=false;

    private void Start()
    {
        if(m_primaryTarget!=null)
        {
            m_currentRotation = m_primaryTarget.transform.localEulerAngles+new Vector3(15,0,0);
        }
    }
    private void FixedUpdate()
    {
        CalculateCamerasView();
    }
    private void CalculateCamerasView()
    {
        Vector3 finalPosition = transform.position;
        Vector3 finalLookDir = transform.forward;


        //Calculate position and rotation according to primary target
        Vector3 targetPosition = m_primaryTarget.transform.position;
        targetPosition.y += m_offsetVertical;

        Vector3 primaryTargetPosition = targetPosition - m_primaryTarget.transform.right * m_offsetHorizontal;
        primaryTargetPosition.y += m_offsetVertical;

        Vector3 offset = Quaternion.Euler(m_currentRotation) * Vector3.forward * m_distance;

        finalPosition = primaryTargetPosition - offset;
        finalPosition = CheckForObstacle(primaryTargetPosition, finalPosition, offset.magnitude);
        finalPosition = Vector3.SmoothDamp(transform.position, finalPosition, ref dampVelocity, m_followSpeed * Time.fixedDeltaTime);

        finalLookDir = (primaryTargetPosition - finalPosition).normalized;

        transform.position = finalPosition;

        transform.rotation = Quaternion.LookRotation(finalLookDir);
    }

    Vector3 CheckForObstacle(Vector3 targetPosition, Vector3 cameraPosition, float distacne)
    {
        RaycastHit hit;
        Vector3 direction = (cameraPosition - targetPosition).normalized;

        Ray ray = new Ray(targetPosition, direction);

        if (Physics.Raycast(ray, out hit, distacne, m_obstacleCheckMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.black, 2);
            return hit.point- direction*1.3f;
        }

        return cameraPosition;
    }

    public void MouseMoved(Vector2 value)
    {
        if (!m_isFocusingOnPOI)
        {
            m_currentRotation.x -= value.y * m_lookSensitivity * Time.fixedDeltaTime;
            m_currentRotation.y += value.x * m_lookSensitivity * Time.fixedDeltaTime;
        }
    }

    public void SetPrimaryTarget(GameObject aTarget) { m_primaryTarget = aTarget; }

    public void ToggleFocusOnInterestPoint()
    {
        m_isFocusingOnPOI = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        Gizmos.DrawWireSphere(transform.position, m_POIDetectionRadius);
    }

}
}

using UnityEngine;

namespace Core.CameraSystem
{
public class TPSFightCamera : BaseCamera
{
    GameObject m_primaryTarget;

    [SerializeField] LayerMask m_obstacleCheckMask;

    [SerializeField] float m_distance;
    [SerializeField] float m_offsetVertical;
    [SerializeField] float m_offsetHorizontal;

    [SerializeField] float m_followSpeed;
    [SerializeField] float m_lookSensitivity;

    Vector3 m_currentRotation;
    Vector3 dampVelocity = new Vector3();

    //bool m_isFocusingOnPOI=false;
   
    private void FixedUpdate()
    {
        CalculateCamerasView();
    }
        //TODO: Add a way to make the camera focus on near by enemies,but still have the player control it
    private void CalculateCamerasView()
    {
        Vector3 finalPosition = transform.position;
        Vector3 finalLookDir = transform.forward;

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
            return hit.point;
        }

        return cameraPosition;
    }

    private int GetPOIIndexWithHighestScore(CameraInterestPoint[] interestPoints)
    {
        Debug.Assert(interestPoints.Length != 0);
        if (interestPoints.Length == 1) return 0;

        int index = 0;
        float currentHighestScore = interestPoints[0].GetPriorityScore();

        for (int i = 1; i < interestPoints.Length; i++)
        {
            if (interestPoints[i].GetPriorityScore() > currentHighestScore)
            {
                index = i;
                currentHighestScore = interestPoints[i].GetPriorityScore();
            }
        }

        return index;
    }

    public void MouseMoved(Vector2 value)
    {
        //if (!m_isFocusingOnPOI)
        //{
            m_currentRotation.x -= value.y * m_lookSensitivity * Time.fixedDeltaTime;
            m_currentRotation.y += value.x * m_lookSensitivity * Time.fixedDeltaTime;
        //}
    }

    bool CheckIfObjectInView(Vector3 worldPosition)
    {
        Vector3 viewPortPosition = GetComponent<Camera>().WorldToViewportPoint(worldPosition);
        float xMin=0.2f,xMax=0.8f;
        float yMin=0.1f,yMax=0.5f;

        if (viewPortPosition.x >= xMin && viewPortPosition.x <= xMax && viewPortPosition.y >= yMin && viewPortPosition.y <= yMax && viewPortPosition.z > 0)
        {
            return true;
        }
        return false;
    }

    public float GetDistance() { return m_distance; }
    public Vector3 GetCurrentRotation() { return m_currentRotation; }
    public void SetDistance(float aDistance) {  m_distance=aDistance; }
    public void SetPrimaryTarget(GameObject aTarget) { m_primaryTarget = aTarget; }

    //public void ToggleFocusOnInterestPoint()
    //{
    //    m_isFocusingOnPOI = true;
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        Gizmos.DrawWireSphere(transform.position, m_POIDetectionRadius);
    }

}
}

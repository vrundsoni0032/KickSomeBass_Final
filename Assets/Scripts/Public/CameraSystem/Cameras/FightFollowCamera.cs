using UnityEngine;

namespace Core.CameraSystem
{
public class FightFollowCamera : BaseCamera
{
    [SerializeField] float m_offsetDistance;
    [SerializeField] float m_offsetFromGround;

    [SerializeField] float m_followSpeed;
    [SerializeField] float m_rotateSpeed;

    Vector3 finalPosition;
    Vector3 finalRotation;

    private void FixedUpdate()
    {
        CalculateCamerasView();
    }

    private void CalculateCamerasView()
    {
        CameraInterestPoint[] interestPoints = FindNearByInterestPoint();
        if (interestPoints == null || interestPoints.Length <2) { YCLogger.Warn("Not enough CameraPOI in the scene for camera to follow"); return; }
        int nearestPOIIndex = NearestPOIIndex(interestPoints);
        
        Vector3 nearestPOIPosition = interestPoints[nearestPOIIndex].transform.position;
        
        int otherIndex = (nearestPOIIndex == 0) ? 1 : 0;
        float distanceFromNearPOI = Vector3.SqrMagnitude(nearestPOIPosition - transform.position);

        //If both POI are inside the view then don't update
        if (!CheckIfObjectInView(interestPoints[0].transform.position) || !CheckIfObjectInView(interestPoints[1].transform.position) || distanceFromNearPOI>20)
        {
            finalRotation = CalculateDirection(nearestPOIPosition, interestPoints[otherIndex].transform.position);
            finalRotation.y = 0;
        
            finalPosition = new Vector3(nearestPOIPosition.x, 0, nearestPOIPosition.z) - (finalRotation * m_offsetDistance);
            finalPosition.y = m_offsetFromGround+ nearestPOIPosition.y;

        }
         Quaternion toRotation = Quaternion.LookRotation(finalRotation);
        
        transform.position = Vector3.Lerp(transform.position, finalPosition, m_followSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, m_rotateSpeed * Time.fixedDeltaTime);
    }

    private int NearestPOIIndex(CameraInterestPoint[] interestPoints)
    {
        int index = 0;
        Vector3 position = interestPoints[0].transform.position;

        for (int i = 1; i < interestPoints.Length; i++)
        {
            if (Vector3.Distance(position, transform.position) > Vector3.Distance(interestPoints[i].transform.position, transform.position))
            {
            index = i;
            }
        }

        return index;
    }

    private Vector3 CalculateDirection(Vector3 nearestPOIPoint, Vector3 otherPOIPoint)
    {
        return (otherPOIPoint-nearestPOIPoint).normalized;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        Gizmos.DrawWireSphere(transform.position, m_POIDetectionRadius);
    }
}
}
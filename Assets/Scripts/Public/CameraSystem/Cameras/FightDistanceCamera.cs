using UnityEngine;

namespace Core.CameraSystem
{
public class FightDistanceCamera : BaseCamera
{
    [SerializeField] bool dynamicZoom;
    [SerializeField] float m_minZoom;
    [SerializeField] float m_maxZoom;

    [SerializeField] float m_followSpeed;

    private void FixedUpdate()
    {
        CalculateCamerasView();
    }

    private void CalculateCamerasView()
    {
        //use dot product or something to make parallel
        float averageDistance = m_minZoom;
        Vector3 averagePOIPosition= GetAveragePositionFromNearPOI();
        if (dynamicZoom)
        {
            averageDistance = GetAverageDistanceBetweenNearPOI();
            averageDistance = Mathf.Clamp(averageDistance, m_minZoom, m_maxZoom);
        }

        //Filter POI that are in range and in view currently instead of just checking if near?

        Vector3 camerasFinalPosition = new Vector3(averagePOIPosition.x, 3, averagePOIPosition.z) + new Vector3(averageDistance, 0, averageDistance);

        transform.position = Vector3.Lerp(transform.position, camerasFinalPosition, m_followSpeed * Time.fixedDeltaTime);

        Vector3 lookDir = averagePOIPosition - transform.position;
        lookDir.y = 0;
        lookDir.Normalize();

        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    private Vector3 GetAveragePositionFromNearPOI()
    {
        CameraInterestPoint[] interestPoints = FindNearByInterestPoint();

        //Return if no POI nearby
        if (interestPoints == null) { return Vector3.zero; }

        Vector3 AvgPOIPosition = Vector3.zero;

        foreach (CameraInterestPoint interestPoint in interestPoints)
        {
            AvgPOIPosition += (interestPoint.transform.position * interestPoint.GetPriorityScore());
        }

        AvgPOIPosition /= interestPoints.Length;


        return AvgPOIPosition;
    }

    private float GetAverageDistanceBetweenNearPOI()
    {
        CameraInterestPoint[] interestPoints = FindNearByInterestPoint();

        //Return if no POI nearby
        if (interestPoints == null) { return -1; }
        float averageDistance = 2;
        
        for (int i = 1; i < interestPoints.Length; i++)
        {
            averageDistance += Vector3.Distance(interestPoints[i - 1].transform.position, interestPoints[i].transform.position);
        }

        return averageDistance;
    }


    private Vector3 CalcPositionFromNearPOI()
    {
        CameraInterestPoint[] interestPoints = FindNearByInterestPoint();

        //Return if no POI nearby
        if (interestPoints == null) { return Vector3.zero; }

        Vector3 position = Vector3.zero;

        if (Vector3.Distance(interestPoints[0].transform.position, transform.position) > Vector3.Distance(interestPoints[1].transform.position, transform.position))
        {
            position = interestPoints[1].transform.position;
        }
        else
        {
            position = interestPoints[0].transform.position;
        }
        return position;
    }

    private Vector3 CalculateDirection()
    {
        CameraInterestPoint[] interestPoints = FindNearByInterestPoint();

        //Return if no POI nearby
        if (interestPoints == null) { return Vector3.zero; }

        Vector3 direction = Vector3.zero;

        if (Vector3.Distance(interestPoints[0].transform.position, transform.position) > Vector3.Distance(interestPoints[1].transform.position, transform.position))
        {
            direction = (interestPoints[0].transform.position - interestPoints[1].transform.position);
        }
        else
        {
            direction = (interestPoints[1].transform.position - interestPoints[0].transform.position);
        }

        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.4f);
        Gizmos.DrawWireSphere(transform.position, m_POIDetectionRadius);


        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Vector3 pos = GetAveragePositionFromNearPOI();

            Gizmos.DrawSphere(pos, 0.5f);
            Gizmos.DrawLine(pos, transform.position);

        }
    }
}
}
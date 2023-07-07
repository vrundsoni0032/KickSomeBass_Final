using System.Collections.Generic;
using UnityEngine;

namespace Core.CameraSystem
{
public class BaseCamera : MonoBehaviour
{
    protected Camera m_camera;
    [SerializeField]string m_name;
    [SerializeField] LayerMask m_collisionLayer;

    [SerializeField] protected float m_POIDetectionRadius; //Range in which camera looks for point of interest (POI)

    protected void Start()
    {
        m_camera = GetComponent<Camera>();
        YCLogger.Assert(this.GetType().Name, m_camera != null, "Camera component not found!");
    }
    private void RegisterToCameraManager(CameraManager aCameraManager)
    {
        aCameraManager.RegisterCamera(this);
    }

    public void Activate() { GetComponent<Camera>().enabled = true; }

    public void Deactivate() { GetComponent<Camera>().enabled = false; }

   protected private CameraInterestPoint[] FindNearByInterestPoint()
   {
        List<CameraInterestPoint> interestPoints = null;

       Collider[] colliders = Physics.OverlapSphere(transform.position, m_POIDetectionRadius, m_collisionLayer);
       
       if (colliders.Length > 0)
       {
           interestPoints = new List<CameraInterestPoint>();
   
           for (int i = 0; i < colliders.Length; i++)
           {
                    //Only add a POI if the score is greater than 0 (below 0 means disabled)
                if(colliders[i].GetComponent<CameraInterestPoint>().GetPriorityScore()>0)
               interestPoints.Add(colliders[i].GetComponent<CameraInterestPoint>());
           }
       }
       else
       { 
           return null;
       }

       return interestPoints.ToArray();
   }

    public string GetName() { return m_name; }
}
}
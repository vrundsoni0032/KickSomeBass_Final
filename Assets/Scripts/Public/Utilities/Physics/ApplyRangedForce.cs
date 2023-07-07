using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtils
{
    public static GameObject[] ApplyForceToObjectInRange(Vector3 position, float radius, float forceAmount, Vector3 forceDirection)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        List<GameObject> hitObjects = new List<GameObject>();

        if (hitColliders.Length > 0)
        {
            foreach (Collider cCollider in hitColliders)
            {
                if (cCollider.GetComponent<Rigidbody>())
                {
                    if (forceDirection == null) //If no direction of force specified,apply for in spherical manner
                    {
                        Vector3 sphericalForceDir = (cCollider.transform.position - position).normalized;
                        cCollider.GetComponent<Rigidbody>().AddForce(sphericalForceDir * forceAmount, ForceMode.VelocityChange);
                    }
                    else
                    {
                        cCollider.GetComponent<Rigidbody>().AddForce(forceDirection * forceAmount, ForceMode.VelocityChange);
                    }
                }
            }
        }

        return hitObjects.ToArray();
    }


    public static GameObject[] ApplyForceToObjectInRange(Vector3 position, float radius, Vector3 forceDirection, float forceAmount, LayerMask mask, GameObject objectToIgnore)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        List<GameObject> hitObjects = new List<GameObject>();

        if (hitColliders.Length > 0)
        {
            foreach (Collider cCollider in hitColliders)
            {
                if (cCollider.GetComponent<Rigidbody>())
                {
                    if (cCollider.gameObject != objectToIgnore)
                    {
                        hitObjects.Add(cCollider.gameObject);
                        if (forceDirection == null|| forceDirection == Vector3.zero) //If no direction of force specified,apply for in spherical manner
                        {
                            Vector3 sphericalForceDir = (cCollider.transform.position - position).normalized;
                            cCollider.GetComponent<Rigidbody>().AddForce(sphericalForceDir * forceAmount, ForceMode.VelocityChange);
                        }
                        else
                        {
                            cCollider.GetComponent<Rigidbody>().AddForce(forceDirection * forceAmount, ForceMode.VelocityChange);
                        }
                    }
                }
            }
        }

        return hitObjects.ToArray();
    }
}

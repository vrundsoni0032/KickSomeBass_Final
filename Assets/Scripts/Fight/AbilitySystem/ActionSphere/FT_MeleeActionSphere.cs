using UnityEngine;
namespace Fight.AbilitySystem
{
class FT_MeleeActionSphere : FT_AbilityActionSphere
{
    protected override void OnInitialized(){}

    private void Update()
    {
        transform.position += forceDirection * Time.deltaTime * 20;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "HitSensor" && other.GetComponentInParent<FT_AbilityUser>() != m_abilityUser)
        {
            if (forceDirection == null)
            {
                forceDirection = m_abilityUser.transform.forward;
            }
            if (other.GetComponent<FT_HitSensorComponent>())
            {
                PhysicsUtils.ApplyForceToObjectInRange(transform.position, GetComponent<SphereCollider>().radius, forceDirection, pushBackForce, LayerMask, m_abilityUser.gameObject);
            }
            Disable();
        }
    }

        
}
}

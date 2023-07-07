using UnityEngine;
namespace Fight.AbilitySystem
{
class FT_ProjectileActionSphere : FT_AbilityActionSphere
{
    public Vector3 moveDirection = Vector3.zero;
    public float m_speed;


    protected override void OnInitialized()
    {
        if (moveDirection == Vector3.zero) moveDirection = transform.forward;
    }

    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime * m_speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.GetComponentInParent<FT_AbilityUser>() != m_abilityUser)
        {
        if (other.tag == "HitSensor" && other.GetComponentInParent<FT_AbilityUser>() != m_abilityUser)
        {
            if (forceDirection == null)
            {
                forceDirection = moveDirection;
            }
            if (other.GetComponent<FT_HitSensorComponent>())
            {

                float size = GetComponent<SphereCollider>().radius;
                Vector3 forceOrigin = transform.position + transform.forward * size / 2;

                GameObject[] obj = PhysicsUtils.ApplyForceToObjectInRange(forceOrigin, size, forceDirection, pushBackForce, LayerMask, m_abilityUser.gameObject);

                if (dontDestroyOnImpact == false)
                {
                    m_speed = 0;
                    Disable();
                }
            }
        }
        }
    }

        
    }
}

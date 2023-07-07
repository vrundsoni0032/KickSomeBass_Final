using System;
using UnityEngine;

namespace Fight.AbilitySystem
{
class FT_SplashProjectileSphere : FT_AbilityActionSphere
{
    public Vector3 moveDirection = Vector3.zero;
    public float m_speed;

    private void Awake()
    {
        if (moveDirection == Vector3.zero) moveDirection = (transform.forward+Vector3.up).normalized;

        if(GetComponent<Rigidbody>()==null) gameObject.AddComponent<Rigidbody>();
    }


    protected override void OnInitialized()
    {
        GetComponent<Rigidbody>().AddForce(moveDirection, ForceMode.VelocityChange);
    }

    public override void Initialize(FT_AbilityUser aAbilityUser, Action<GameObject> OnImpactCallBack, float aDamageAmount, ProjectileFXData aFXData, float aLifeTime = 0.1f)
    {
        base.Initialize(aAbilityUser, OnImpactCallBack, aDamageAmount, aFXData, aLifeTime);

        GetComponent<Rigidbody>().AddForce(moveDirection, ForceMode.VelocityChange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.GetComponentInParent<FT_AbilityUser>() != m_abilityUser)
        {
        if (other.GetComponent<FT_HitSensorComponent>() && other.GetComponent<FT_AbilityUser>() != m_abilityUser)
        {
            GameObject[] obj = PhysicsUtils.ApplyForceToObjectInRange(transform.position, GetComponent<SphereCollider>().radius, forceDirection, pushBackForce, LayerMask, m_abilityUser.gameObject);

            TriggerSurfaceParticleEffect();

            if (dontDestroyOnImpact == false)
            {
                Disable();
            }
        }
        }
    }
}


}
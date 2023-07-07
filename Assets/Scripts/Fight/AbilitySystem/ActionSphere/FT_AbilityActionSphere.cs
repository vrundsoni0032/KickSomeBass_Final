using System;
using UnityEngine;

namespace Fight.AbilitySystem
{
    [System.Serializable]
public class ProjectileFXData
{
    public GameObject projectleParticleEffect;
    public GameObject surfaceParticleEffect;
    public AudioClip sfx;

}

//FT_AbilityActionSphere is a base class for objects to be used by abilities to apply damage,force etc on colliding(being in range of ability).  
public abstract class FT_AbilityActionSphere : MonoBehaviour
{
    protected FT_AbilityUser m_abilityUser;

    public float lifeTime = 0.1f;
    public float pushBackForce;
    public float forceSphereSize = 2;
    public float damageAmount = 0;

    public Vector3 forceDirection;

    public LayerMask LayerMask = ~0;

    ProjectileFXData projectileFXData;

    public bool dontDestroyOnImpact = false;

    protected Action<GameObject> DisableOnImpact;

    public virtual void Initialize(FT_AbilityUser aAbilityUser,Action<GameObject> OnImpactCallBack, float aDamageAmount,float aLifeTime=0.1f)
    {
        m_abilityUser = aAbilityUser;
        damageAmount = aDamageAmount;
        lifeTime = aLifeTime;
        DisableOnImpact=OnImpactCallBack;
        GameUtil.TriggerTimer.CreateTimer(()=> { if (DisableOnImpact == null) return; DisableOnImpact.Invoke(gameObject); }, aLifeTime);//Invoke DisableOnImpact after certain time

        OnInitialized();
    }

    public virtual void Initialize(FT_AbilityUser aAbilityUser, Action<GameObject> OnImpactCallBack, float aDamageAmount, ProjectileFXData aFXData, float aLifeTime=0.1f)
    {
        Initialize(aAbilityUser,OnImpactCallBack,aDamageAmount,aLifeTime);
        
        projectileFXData = aFXData;
    }

    protected abstract void OnInitialized();//Function for child class to use to apply any custom intial behaviour
   
    protected void Disable() 
    {
        if (DisableOnImpact != null)
            DisableOnImpact.Invoke(gameObject);
        DisableOnImpact = null;
    }

    public FT_AbilityUser GetAbilityUser() { return m_abilityUser; }

    public float GetDamageAmount() { return damageAmount;}

    protected void TriggerProjectileParticle()
    {
        if (projectileFXData.projectleParticleEffect == null)
            return;

        GameObject particleObject = Instantiate(projectileFXData.projectleParticleEffect);
        particleObject.transform.localPosition = transform.position;

        Vector3 rotation = m_abilityUser.transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;

        particleObject.transform.rotation = Quaternion.Euler(rotation);
        particleObject.GetComponent<MoveScript>().direction = m_abilityUser.transform.forward;
    }

    protected void TriggerSurfaceParticleEffect()
    {
        if (projectileFXData.surfaceParticleEffect == null)
        return;

        GameObject particleObject = Instantiate(projectileFXData.surfaceParticleEffect);
        particleObject.transform.localPosition = transform.position;
        particleObject.GetComponent<ParticleSystem>().Play();

    }

    protected void TriggerSoundEffect(AudioClip audioClip)
    {

    }
}


}
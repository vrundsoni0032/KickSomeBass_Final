using UnityEngine;

namespace Fight.AbilitySystem
{
public enum FT_AbilityState { None, Begin, InProcess, End }

public abstract class FT_Ability : ScriptableObject
{
    public string Name;
    public float Range;
    public float Damage;
    public float StaminaCost;

    [HideInInspector] public Vector3 Direction;
    [SerializeField]GameObject prfb_actionSphere;
    public ProjectileFXData FXData;

    public abstract void Begin(FT_AbilityUser abilityUser);
    public abstract void InProcess(FT_AbilityUser abilityUser);
    public abstract void End(FT_AbilityUser abilityUser);

    public GameObject GetActionSpherePrefab() { return prfb_actionSphere; }
    
    protected GameObject SpawnActionSphere(Vector3 aPosition,Quaternion aRotation, float detectionRadius = 0.1f)
    {
        GameObject instance=ObjectPoolManager.Spawn(prfb_actionSphere, aPosition, aRotation);

        instance.GetComponent<SphereCollider>().radius = detectionRadius;

        return instance;
    }

    protected void DisableActionSphere(GameObject aInstance)
    {
        ObjectPoolManager.DeSpawn(prfb_actionSphere,aInstance);
    }
}
}
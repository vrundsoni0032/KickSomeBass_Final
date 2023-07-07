using UnityEngine;

namespace Fight.AbilitySystem
{
    [CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/BigSuccAbility")]
    public class FT_WSBigSucc : FT_Ability
    {
        // Start is called before the first frame update
        public override void Begin(FT_AbilityUser abilityUser)
        {
            YCLogger.Info("BigSucc Begin");
            abilityUser.SetState(FT_AbilityState.InProcess);
        }

        public override void InProcess(FT_AbilityUser abilityUser)
        {
            LayerMask mask = ~0;
            Vector3 position = abilityUser.transform.position + abilityUser.transform.forward * Range;

            GameObject actionSphereObj = SpawnActionSphere(position,Quaternion.identity, 10);
            FT_ProjectileActionSphere actionSphere = actionSphereObj.GetComponent<FT_ProjectileActionSphere>();
            //Get JimJimson and pull him towards WhaleShark
            GameObject jimJimson = new GameObject();
            jimJimson = GameObject.FindWithTag("Fish");
            jimJimson.transform.position = abilityUser.transform.position;

            actionSphere.pushBackForce = 20;
            actionSphere.forceDirection = abilityUser.transform.forward;
            actionSphere.LayerMask = mask; 
            actionSphere.lifeTime = 1.0f;

            actionSphere.Initialize(abilityUser, DisableActionSphere, Damage);

            abilityUser.SetState(FT_AbilityState.End);
        }

        public override void End(FT_AbilityUser abilityUser)
        {
            YCLogger.Info("BigSucc End");
            abilityUser.SetState(FT_AbilityState.None);
            
        }
    }
}
using System;
using UnityEngine;

namespace Fight.AbilitySystem
{
[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/ItemAbility/BootThrow")]
public class FT_BootThrowAbility : FT_Ability
{
    FT_AbilityUser AbilityUser;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        abilityUser.transform.rotation = Quaternion.Euler(0,Quaternion.LookRotation(Direction).eulerAngles.y,0);
        GameUtil.EventManager.AddEvent(new Events.FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), true, true, true, true));
        GameUtil.EventManager.AddEvent(new Events.FT_PlayAnimationEvent(abilityUser.GetComponent<Fighter.FT_Fighter>(), "throw", ThrowOnAnimEvent));
        AbilityUser = abilityUser;

        abilityUser.SetState(FT_AbilityState.InProcess);
    }

    private void ThrowOnAnimEvent(string aAction)
    {
        if (aAction == "throw")
        {
            Vector3 position = AbilityUser.transform.position + AbilityUser.transform.forward+Vector3.up;

            GameObject actionSphereObj = SpawnActionSphere(position, Quaternion.identity, 20);
            FT_ProjectileActionSphere actionSphere = actionSphereObj.GetComponent<FT_ProjectileActionSphere>();

            actionSphereObj.transform.forward=AbilityUser.transform.forward;

            actionSphere.pushBackForce = 10;
            actionSphere.forceDirection = AbilityUser.transform.forward;
            actionSphere.m_speed = 30;
            actionSphere.dontDestroyOnImpact = true;
            actionSphere.Initialize(AbilityUser, DisableActionSphere, Damage,3);

            GameUtil.EventManager.AddEvent(new Events.FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), false, false, false, false));

            AbilityUser.SetState(FT_AbilityState.End);
        }
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        abilityUser.SetState(FT_AbilityState.None);
    }
}
}

using Fight.Events;
using UnityEngine;

namespace Fight.AbilitySystem
{

[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/PunchAbility")]
class FT_PunchAbility : FT_Ability
{
    FT_AbilityUser AbilityUser;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        abilityUser.transform.rotation = Quaternion.Euler(0,Quaternion.LookRotation(Direction).eulerAngles.y,0);
        abilityUser.SetState(FT_AbilityState.InProcess);    

        GameUtil.EventManager.AddEvent(new FT_PlayAnimationEvent(abilityUser.GetComponent<Fighter.FT_Fighter>(), "punch", PunchOnAnimEvent));
        AbilityUser = abilityUser;
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        
    }

    public void PunchOnAnimEvent(string aEventAction)
    {
        if (aEventAction != "punch") return;
        GameUtil.SoundManager.PlaySound("PlayerPunch");

        Vector3 position = AbilityUser.transform.position + AbilityUser.transform.forward * Range;

        GameObject actionSphereObj = SpawnActionSphere(position,Quaternion.identity, 4);
        FT_MeleeActionSphere actionSphere = actionSphereObj.GetComponent<FT_MeleeActionSphere>();

        actionSphere.pushBackForce = 2;
        actionSphere.forceDirection = AbilityUser.transform.forward;
        actionSphere.lifeTime = .1f;

        actionSphere.Initialize(AbilityUser, DisableActionSphere, Damage, FXData);

        AbilityUser.SetState(FT_AbilityState.End);
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        abilityUser.SetState(FT_AbilityState.None);
    }
}   
}
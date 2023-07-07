using UnityEngine;
using Fight.Fighter;

namespace Fight.AbilitySystem
{
[CreateAssetMenu(fileName = "HeadBash", menuName = "KSBUtilities/Fight/Ability/HeadBashAbility")]
class FT_HeadBash : FT_Ability
{
    [SerializeField]float forceAmount;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        abilityUser.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(Direction).eulerAngles.y, 0);
        GameUtil.EventManager.AddEvent(new Events.FT_PlayAnimationEvent(abilityUser.GetComponent<Fighter.FT_Fighter>(), "HeadBash"));
        GameUtil.EventManager.AddEvent(new Events.FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), true, true, true, true));

        abilityUser.SetState(FT_AbilityState.InProcess);
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        abilityUser.gameObject.GetComponent<FT_Fighter>().AddForce(Direction, forceAmount);
        abilityUser.SetState(FT_AbilityState.End);
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        Vector3 position = abilityUser.transform.position + abilityUser.transform.forward;

        GameObject actionSphereObj=SpawnActionSphere(position,Quaternion.identity,5);
        FT_MeleeActionSphere actionSphere = actionSphereObj.GetComponent<FT_MeleeActionSphere>();
            
        actionSphere.pushBackForce = 100;
        actionSphere.forceDirection = abilityUser.transform.forward;
        
        actionSphere.Initialize(abilityUser, DisableActionSphere, Damage);
        abilityUser.SetState(FT_AbilityState.None);

        GameUtil.EventManager.AddEvent(new Events.FT_FreezeFighterActionEvent(FightUtil.GetPlayer(), false, false, false, false));

    }
}
}
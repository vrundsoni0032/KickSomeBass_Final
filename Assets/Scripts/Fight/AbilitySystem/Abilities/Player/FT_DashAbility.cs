using Core.Component;
using UnityEngine;

namespace Fight.AbilitySystem
{
[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/DashAbility")]
class FT_DashAbility : FT_Ability
{
    [SerializeField] float dashForce;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        abilityUser.SetState(FT_AbilityState.InProcess);
        
        if(Direction == Vector3.zero)
        {
            Direction = abilityUser.transform.forward;
        }

        abilityUser.transform.rotation = Quaternion.Euler(0,Quaternion.LookRotation(Direction).eulerAngles.y,0);
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        abilityUser.GetComponent<MovementComponent>().ApplyForce(Direction, dashForce);
        abilityUser.SetState(FT_AbilityState.End);
    }

    public override void End(FT_AbilityUser abilityUser)
    {
            abilityUser.SetState(FT_AbilityState.None);
    }
}
}
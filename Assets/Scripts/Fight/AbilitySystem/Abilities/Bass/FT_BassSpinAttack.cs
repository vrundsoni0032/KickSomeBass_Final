using UnityEngine;

namespace Fight.AbilitySystem
{

[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/BassSpinAttack")]
class FT_BassSpinAttack : FT_Ability
{
    FT_AbilityUser m_abilityUser;
    bool bWaitingForAnimation = false;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        //YCLogger.Info("BassSpinAttack Begin");
        abilityUser.transform.rotation = Quaternion.Euler(0,Quaternion.LookRotation(Direction).eulerAngles.y,0);
        abilityUser.SetState(FT_AbilityState.InProcess);
        GameUtil.EventManager.AddEvent(new Events.FT_PlayAnimationEvent(abilityUser.GetComponent<Fighter.FT_Fighter>(), "spinAttack", AttackhOnAnimEvent));
             
       // GameUtil.TriggerTimer.CreateTimer(AttackhOnAnimEvent, 0.2f);
        bWaitingForAnimation = true;
        m_abilityUser = abilityUser;
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        if(bWaitingForAnimation)
        {
            return;
        }

        abilityUser.SetState(FT_AbilityState.End);
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        //YCLogger.Info("Punch End");
        abilityUser.SetState(FT_AbilityState.None);
    }

    public void AttackhOnAnimEvent(string aAction)
    {
        if (aAction == "spinAttack")
        {
            LayerMask mask = ~0;
            //To do:check if during animation the character transform will spin or not,then decide the direction 
            Vector3 position = m_abilityUser.transform.position + m_abilityUser.transform.forward;

            GameObject actionSphereObj = SpawnActionSphere(position,Quaternion.identity, Range);
            FT_MeleeActionSphere actionSphere = actionSphereObj.GetComponent<FT_MeleeActionSphere>();

            Vector3 forceDirection = (m_abilityUser.transform.forward + m_abilityUser.transform.right).normalized;

            actionSphere.forceDirection = forceDirection;
            actionSphere.pushBackForce = 5;
            actionSphere.LayerMask = mask;

            actionSphere.Initialize(m_abilityUser, DisableActionSphere, Damage, 0.2f);

            m_abilityUser.SetState(FT_AbilityState.End);
            bWaitingForAnimation = false;
        }
    }
}
}
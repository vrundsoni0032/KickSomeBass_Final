using UnityEngine;

namespace Fight.AbilitySystem
{

[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/BassStrumAbility")]
class FT_BassStrumAbility : FT_Ability
{
    FT_AbilityUser m_abilityUser;

    bool bWaitingForAnimation=false;

    [SerializeField]GameObject pfx_musicNotes;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        //YCLogger.Info("BassStrumAbility Begin");
        abilityUser.SetState(FT_AbilityState.InProcess);

        m_abilityUser = abilityUser;
        bWaitingForAnimation = true;
        abilityUser.transform.rotation = Quaternion.Euler(0,Quaternion.LookRotation(Direction).eulerAngles.y,0);
        GameUtil.TriggerTimer.CreateTimer(PerformAction, 0.2f); //Temp:Wait for animation
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        if(bWaitingForAnimation) return;

        abilityUser.SetState(FT_AbilityState.End);
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        //YCLogger.Info("Punch End");
        abilityUser.SetState(FT_AbilityState.None);
    }

    public void PerformAction()
    {
        Vector3 position = m_abilityUser.transform.position + m_abilityUser.transform.forward+Vector3.up*.5f;

        GameObject actionSphereObj = SpawnActionSphere(position,Quaternion.identity, Range);
        FT_ProjectileActionSphere actionSphere = actionSphereObj.GetComponent<FT_ProjectileActionSphere>();

        Vector3 forceDirection= (m_abilityUser.transform.forward + (m_abilityUser.transform.up * 0.3f) * 0.5f).normalized;

        actionSphere.forceDirection = forceDirection;
        actionSphere.moveDirection = m_abilityUser.transform.forward;
        actionSphere.pushBackForce = 5;
        actionSphere.m_speed = 15;

        actionSphere.Initialize(m_abilityUser,DisableActionSphere, Damage, FXData,5);
        Instantiate(pfx_musicNotes,position,Quaternion.identity);

        m_abilityUser.SetState(FT_AbilityState.End);
        bWaitingForAnimation = false;
    }
}
}
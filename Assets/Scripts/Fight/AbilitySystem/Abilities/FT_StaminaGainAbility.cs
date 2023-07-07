using UnityEngine;
using Fight.Events;
using Fight.Fighter;

namespace Fight.AbilitySystem
{
[CreateAssetMenu(fileName = "FT_NewAbility", menuName = "KSBUtilities/Fight/Ability/StaminaGainAbility")]
class FT_StaminaGainAbility : FT_Ability
{
    [SerializeField] private float m_minGain = 0.1f;
    [SerializeField] private float m_maxGain = 3.0f;
    [SerializeField] private float m_accelerationTime = 3.0f;

    private float m_timeSinceBegin = 0.0f;

    public override void Begin(FT_AbilityUser abilityUser)
    {
        m_timeSinceBegin = 0.0f;    
        GameUtil.EventManager.AddEvent(new FT_FreezeFighterActionEvent(abilityUser.GetComponent<FT_Fighter>(), true, true, false, false));

        abilityUser.SetState(FT_AbilityState.InProcess);
    }

    public override void InProcess(FT_AbilityUser abilityUser)
    {
        m_timeSinceBegin += Time.deltaTime ;

        float gainAmount = Mathf.SmoothStep(m_minGain, m_maxGain, m_timeSinceBegin / m_accelerationTime );
        GameUtil.EventManager.AddEvent(new FT_ChangeStaminaEvent(abilityUser.GetComponent<FT_Fighter>(), gainAmount));
    }

    public override void End(FT_AbilityUser abilityUser)
    {
        m_timeSinceBegin = 0.0f;
        abilityUser.SetState(FT_AbilityState.None);
        GameUtil.EventManager.AddEvent(new FT_FreezeFighterActionEvent(abilityUser.GetComponent<FT_Fighter>(),
            false, false, false, false));
    }
}
}
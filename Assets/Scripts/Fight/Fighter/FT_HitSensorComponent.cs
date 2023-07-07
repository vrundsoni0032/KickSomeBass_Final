using Fight.AbilitySystem;
using Fight.Fighter;
using Fight.Events;
using UnityEngine;

namespace Fight
{
public class FT_HitSensorComponent : MonoBehaviour
{
    [SerializeField]FT_AbilityUser m_abilityUser;

    private void OnTriggerEnter(Collider other)
    {
        FT_AbilityActionSphere abilityActionSphere = other.GetComponent<FT_AbilityActionSphere>();
        if (abilityActionSphere == null) { return; }

        if (FightUtil.FightManager == null) { return; }
        if (!FightUtil.FightManager.IsRoundInProcess()) { return; }
    
        FT_Fighter otherFighter = abilityActionSphere.GetAbilityUser().GetComponent<FT_Fighter>();

        //Check if not the caller
        if (otherFighter == m_abilityUser.GetComponent<FT_Fighter>()) { return; }

        if (m_abilityUser.GetComponent<FT_Fighter>().CanGetHit()==false) { return; }

        GameUtil.EventManager.AddEvent(new FT_PlayAnimationEvent(m_abilityUser.GetComponent<Fighter.FT_Fighter>(), "hurt"));

        GameUtil.EventManager.AddEvent(new FT_FreezeFighterActionEvent(m_abilityUser.GetComponent<FT_Fighter>(), true, true, false, false));

        GameUtil.EventManager.AddEvent(new 
        FT_ChangeHealthEvent(m_abilityUser.GetComponent<FT_Fighter>(), -abilityActionSphere.GetDamageAmount()));
        //Show hit art when hit
        GameUtil.EventManager.AddEvent(new
        FT_ShowDamageUIEvent(m_abilityUser.GetComponent<FT_Fighter>()));
        //Stunts the power to use any ability // Should take response animation timer in consideration.

        GameUtil.TriggerTimer.CreateTimer(() =>
        {
            if (!FightUtil.FightManager.IsRoundInProcess()) { return; }
            GameUtil.EventManager.AddEvent(new FT_FreezeFighterActionEvent(m_abilityUser.GetComponent<FT_Fighter>(), false, false, false, false));
        }, .1f);
    }
}
}
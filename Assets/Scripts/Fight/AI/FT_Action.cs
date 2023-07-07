using UnityEngine;

namespace Fight.AI
{
public enum FT_ActionType { Regular, Filler }

[System.Serializable] public struct FT_Action
{
    [SerializeField] private string m_actionName;
    [SerializeField] private float m_distanceCovered;
    [SerializeField] private FT_ActionType m_ActionType;
    [SerializeField] private float m_coolDown;
    [Range(0.0f, 1.0f), SerializeField] private float m_engagementPoints;

    public string ActionName => m_actionName;
    public FT_ActionType ActionType => m_ActionType;
    public float DistanceCovered => m_distanceCovered;
    public float CoolDown => m_coolDown;

    public float GetScore(float stamina, float damage, float distance)
    {
        switch (m_ActionType)
        {
            case FT_ActionType.Filler:
                return m_engagementPoints;

            case FT_ActionType.Regular:

                if (m_actionName == "AIStaminaGain")
                {
                    return FightUtil.AIBrain.StaminaGainConsideration.GetScore(10.0f / FightUtil.GetAI().Stamina) + m_engagementPoints;
                }
                else if (distance > FightUtil.AIAbilityUser.GetAbility(m_actionName).Range)
                {
                    return m_engagementPoints - FightUtil.AIBrain.StaminaCostConsideration.GetScore(stamina / FightUtil.GetAI().Stamina);
                }
                return m_engagementPoints - FightUtil.AIBrain.StaminaCostConsideration.GetScore(stamina / FightUtil.GetAI().Stamina)
                       + FightUtil.AIBrain.PlayerDamageConsideration.GetScore(damage / FightUtil.GetPlayer().GetMaxHealth());
        }

        YCLogger.Assert("FT_Action", false, "Unknown score state triggered. Action Name - " + m_actionName);
        return 0.0f;
    }
}
}
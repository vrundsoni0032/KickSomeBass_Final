using UnityEngine;

namespace InteractiveMap.SkillTree
{
public class IM_SkillTraits : MonoBehaviour
{
    public void HealthTrait(float gainAmount) =>
        GameUtil.PlayerState.MaxHealth += GameUtil.PlayerState.MaxHealth * (gainAmount / 100.0f);

    public void StaminaTrait(float gainAmount) =>
        GameUtil.PlayerState.MaxStamina += GameUtil.PlayerState.MaxStamina * (gainAmount / 100.0f);
    
    public void StrengthTrait(float gainAmount) =>
        GameUtil.PlayerState.MaxStrength += GameUtil.PlayerState.MaxStrength * (gainAmount / 100.0f);

    public void SpeedTrait(float gainAmount) =>
        GameUtil.PlayerState.MaxSpeed += GameUtil.PlayerState.MaxSpeed * (gainAmount / 100.0f);

    public void AddAbility(string name) =>
        GameUtil.PlayerState.AbilityList.Add(name);

    public void UpgradeHeavyPunch() =>
        YCLogger.Debug("Skill trait", "Upgraded Heavy Punch.");

    public void UpgradeHeadBash() =>
        YCLogger.Debug("Skill trait", "Upgraded Head Bash.");

    public void UpgradeKnockOut() =>
        YCLogger.Debug("Skill trait", "Upgraded Knock out.");

    public void UpgradeGroundSmash() =>
        YCLogger.Debug("Skill trait", "Upgraded Ground Smash.");

    public void UpgradeSpinThrow() =>
        YCLogger.Debug("Skill trait", "Upgraded Spin Throw.");
}
}
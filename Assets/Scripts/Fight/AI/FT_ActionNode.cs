using System.Collections.Generic;
using Fight.AbilitySystem;
using UnityEngine;

namespace Fight.AI
{
public class FT_ActionNode
{
    public readonly FT_Action Action;
    public readonly FT_ActionNode ParentNode;
    public readonly List<FT_ActionNode> ChildNodes;

    public readonly float CurrentScore;
    public readonly float CurrentStamina;
    public readonly float CurrentDistance;

    public FT_ActionNode(FT_ActionNode parentNode, FT_Action action, float currentScore, float currentDistance, float currentStamina)
    {
        Action = action;
        ParentNode = parentNode;
        ChildNodes = new List<FT_ActionNode>();

        if (action.ActionType == FT_ActionType.Regular)
        {
            FT_Ability ability = FightUtil.AIAbilityUser.GetAbility(action.ActionName);
            CurrentScore = currentScore + action.GetScore(ability.StaminaCost, ability.Damage, currentDistance);
            CurrentStamina = currentStamina - ability.StaminaCost;
            CurrentDistance = currentDistance - Action.DistanceCovered;
        }
        else
        {
            CurrentScore = currentScore + action.GetScore(0.0f, 0.0f, currentDistance);
            CurrentStamina = currentStamina;
            CurrentDistance = currentDistance - Action.DistanceCovered;
        }

        CurrentDistance = Mathf.Clamp(CurrentDistance, 0.0f, FightUtil.Arena.GetArenaDiameterInMeter());
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0.0f, FightUtil.GetAI().GetMaxStamina());
    }

    public void AttachChildren(FT_Action action)
    {
        switch (action.ActionType)
        {
            case FT_ActionType.Filler:
                if (FT_ActionNodeUtil.IsFillerActionPossible(action.ActionName, CurrentDistance))
                {
                    ChildNodes.Add(new FT_ActionNode(this, action, CurrentScore, CurrentDistance, CurrentStamina));
                }
                break;

            case FT_ActionType.Regular:
                if (FT_ActionNodeUtil.IsRegularActionPossible(action.ActionName, CurrentStamina))
                {
                    ChildNodes.Add(new FT_ActionNode(this, action, CurrentScore, CurrentDistance, CurrentStamina));
                }
                break;
        }
    }
}
}
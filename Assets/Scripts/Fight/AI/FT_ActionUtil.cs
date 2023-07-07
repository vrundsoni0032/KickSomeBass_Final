using System.Collections.Generic;

namespace Fight.AI
{
public static class FT_ActionNodeUtil
{
    public static bool IsFillerActionPossible(string actionName, float currentDistance)
    {
        switch (actionName)
        {
            case "AIMoveIn":
                return currentDistance > FightUtil.GetAI().MinDistanceFromPlayer;

            case "AIMoveAway":
                return currentDistance < FightUtil.GetAI().MaxDistanceFromPlayer;
        }

        return true;
    }

    public static bool IsRegularActionPossible(string actionName, float currentStamina)
    {
        return currentStamina > FightUtil.AIAbilityUser.GetAbility(actionName).StaminaCost;
    }

    public static FT_ActionNode GetHighestScoringNode(List<FT_ActionNode> actionNodes)
    {
        float highestScore = 0.0f;
        FT_ActionNode highestNode = actionNodes[0];

        foreach (var actionNode in actionNodes)
        {
            float currentNodeScore = GetHighestScore(actionNode);

            if (highestScore < currentNodeScore)
            {
                highestScore = currentNodeScore;
                highestNode = actionNode;
            }
        }

        return highestNode;
    }

    public static float GetHighestScore(FT_ActionNode topNode)
    {
        List<FT_ActionNode> leafNodes = new List<FT_ActionNode>();
        GetLeafNodes(topNode, ref leafNodes);

        float highScore = 0.0f;

        foreach (var leafNode in leafNodes)
        {
            if (highScore < leafNode.CurrentScore)
            {
                highScore = leafNode.CurrentScore;
            }
        }

        return highScore;
    }

    public static void GetLeafNodes(FT_ActionNode topNode, ref List<FT_ActionNode> leafNodes)
    {
        if (topNode.ChildNodes.Count == 0)
        {
            leafNodes.Add(topNode);
            return;
        }

        foreach (var childNode in topNode.ChildNodes)
        {
            GetLeafNodes(childNode, ref leafNodes);
        }
    }
}
}
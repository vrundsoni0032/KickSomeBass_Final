using System.Collections;
using System.Collections.Generic;
using Fight.AbilitySystem;
using Fight.Events;
using UnityEngine;

namespace Fight.AI
{
public class FT_Brain : MonoBehaviour
{
    //[SerializeField] private float m_obstacleDetectionRange = 4.0f;

    [SerializeField] private uint m_evaluationDepth = 5;
    [SerializeField] private float m_waitTime = 0.25f;

    public FT_Consideration PlayerDamageConsideration;
    public FT_Consideration StaminaCostConsideration;
    public FT_Consideration StaminaGainConsideration;

    [SerializeField] private List<FT_Action> m_defensiveActions; //Will be only considered at depth 0, if in danger.
    [SerializeField] private List<FT_Action> m_regularActions;
    [SerializeField] private List<FT_Action> m_comboActions;

    private List<FT_ActionNode> m_TopNodes = new List<FT_ActionNode>();
    private List<FT_Action> m_lastActions = new List<FT_Action>();
    private int m_depth = 0;
    private FT_Action m_bestAction;

    private Coroutine m_processCoroutine;

    private Dictionary<string, float> m_possibleCoolDownActions = new Dictionary<string, float>();
    private List<FT_CoolDownAction> m_ActionsInCoolDown = new List<FT_CoolDownAction>();

    private void Awake()
    {
        foreach (var defensiveAction in m_defensiveActions)
        {
            if (defensiveAction.CoolDown > Mathf.Epsilon)
            {
                m_possibleCoolDownActions.Add(defensiveAction.ActionName, defensiveAction.CoolDown);
                //m_possibleCoolDownActions.Add(defensiveAction);
            }
        }

        foreach (var regularAction in m_regularActions)
        {
            if (regularAction.CoolDown > Mathf.Epsilon)
            {
                m_possibleCoolDownActions.Add(regularAction.ActionName, regularAction.CoolDown);
            }
        }
    }

    private void Update()
    {
        foreach (var actionInCoolDown in m_ActionsInCoolDown)
        {
            actionInCoolDown.Timer -= Time.deltaTime;
        }

        m_ActionsInCoolDown.RemoveAll(action => action.Timer < 0.0f);
    }

    public void StartProcess()
    {
        m_depth = 0;
        m_TopNodes.Clear();
        m_processCoroutine = StartCoroutine(Process());
    }

    public void StopProcess()
    {
        m_depth = 0;
        m_TopNodes.Clear();
        m_lastActions.Clear();
        m_ActionsInCoolDown.Clear();

        if (m_processCoroutine != null)
        {
            StopCoroutine(m_processCoroutine);
        }
    }

    private IEnumerator Process()
    {
        foreach (var comboAction in m_comboActions)
        {
            if (IsComboPossible(comboAction.ActionName, null))
            {
                if (FT_ActionNodeUtil.IsRegularActionPossible(comboAction.ActionName, FightUtil.GetAI().Stamina))
                {
                    m_TopNodes.Add(new FT_ActionNode(null, comboAction, 0.0f, FightUtil.GetDistance(), FightUtil.GetAI().Stamina));
                }
            }
        }

        if (m_TopNodes.Count == 0)
        {
            foreach (FT_Action action in IsBlockingNecessary() ? m_defensiveActions : m_regularActions)
            {
                if (!IsPossibleInitialAction(action)) { continue; }

                m_TopNodes.Add(new FT_ActionNode(null, action, 0.0f, FightUtil.GetDistance(), FightUtil.GetAI().Stamina));
            }
        }

        m_bestAction = FT_ActionNodeUtil.GetHighestScoringNode(m_TopNodes).Action;
        m_depth++;

        yield return null;
        
        while (m_depth < m_evaluationDepth)
        {
            foreach (var topNode in m_TopNodes)
            {
                List<FT_ActionNode> leafNodes = new List<FT_ActionNode>();
                FT_ActionNodeUtil.GetLeafNodes(topNode, ref leafNodes);

                foreach (var leafNode in leafNodes)
                {
                    foreach (var comboAction in m_comboActions)
                    {
                        if (IsComboPossible(comboAction.ActionName, leafNode))
                        {
                            leafNode.AttachChildren(comboAction);
                            break;
                        }
                    }
                    if (leafNode.ChildNodes.Count > 0) { break; }

                    foreach (var regularAction in m_regularActions)
                    {
                        if (IsActionPossibleAtCurrentDepth(regularAction.ActionName))
                        {
                            leafNode.AttachChildren(regularAction);
                        }
                    }
                }

                yield return null;
            }

            m_bestAction = FT_ActionNodeUtil.GetHighestScoringNode(m_TopNodes).Action;
            m_depth++;


            yield return null;
        }

        // In case of Combo action...combo handler will create the actual event.
        //if (m_bestAction.ActionType != FT_ActionType.Combo)
        {
            GameUtil.EventManager.AddEvent(new FT_AIInputEvent(m_bestAction.ActionName));
        }

        YCLogger.Info("AIBrain", "Selected action - " + m_bestAction.ActionName);
        AddLastAction(m_bestAction);

        yield return new WaitForSeconds(m_waitTime);

        StartProcess();

        yield return null;
    }

    public bool IsActionPossibleAtCurrentDepth(string actionName)
    {
        if (m_possibleCoolDownActions.ContainsKey(actionName))
        {
            if(m_depth > 4) { return true; }
            if(m_depth > 0) { return false; }

            FT_CoolDownAction action = m_ActionsInCoolDown.Find(actionInCoolDown => actionInCoolDown.ActionName == actionName);

            if (action != null)
            {
                if (action.Timer > m_depth * m_waitTime) { return false; }
            }
        }
        return true;
    }

    // For optimization.
    private bool IsPossibleInitialAction(FT_Action action)
    {
        if(!IsActionPossibleAtCurrentDepth(action.ActionName)) { return false; }

        switch (action.ActionType)
        {
            case FT_ActionType.Filler:
                return FT_ActionNodeUtil.IsFillerActionPossible(action.ActionName, FightUtil.GetDistance());

            case FT_ActionType.Regular:
                if (action.ActionName == "AIStaminaGain") { if(FightUtil.GetAI().Stamina + Mathf.Epsilon > 100.0f) { return false; } }
                return FT_ActionNodeUtil.IsRegularActionPossible(action.ActionName, FightUtil.GetAI().Stamina);
        }

        return true;
    }

    private bool IsBlockingNecessary()
    {
        FT_Ability playerCurrentAbility = FightUtil.PlayerAbilityUser.CurrentAbility;

        if(playerCurrentAbility == null) { return false;}
        if (playerCurrentAbility.Damage < Mathf.Epsilon) { return false; }
        if (FightUtil.GetDistance() < playerCurrentAbility.Range) { return false; }

        //TODO - Is any projectile in zone.

        return true;
    }

    private bool IsBlockEffective()
    {
        //TODO - Should ask the combat system passing in the future position if the blocking works or no?
        return true;
    }

    public void GetLastActions(int actionCount, ref List<string> actions)
    {
        if(m_lastActions.Count < actionCount) { return; }

        for (int i = 0; i < actionCount; i++)
        {
            actions.Add(m_lastActions[m_lastActions.Count - actionCount + i].ActionName);
        }
    }

    //Most recent moves goes to the end.
    private void AddLastAction(FT_Action action)
    {
        if (m_lastActions.Count >= m_evaluationDepth)
        {
            m_lastActions.RemoveAt(0);
        }

        m_lastActions.Add(action);

        string debug = "Last Actions:";
        foreach (var lastAction in m_lastActions)
        {
            debug += " " + lastAction.ActionName + ", ";
        }

        YCLogger.Debug("AIBrain", debug);
        if (m_possibleCoolDownActions.ContainsKey(action.ActionName))
        {
            // Should throw assert as the code beforehand should not allow the action already in cool down list to be selected again.
            YCLogger.Assert("AIBrain", m_ActionsInCoolDown.Find(actionInCoolDown => actionInCoolDown.ActionName == action.ActionName) == null,
                $"Trying to add the {action.ActionName} twice before the previous cool down is up.");

            m_ActionsInCoolDown.Add(new FT_CoolDownAction(action.ActionName, m_possibleCoolDownActions[action.ActionName]));
        }
    }

    private bool IsComboPossible(string comboName, FT_ActionNode recentNode)
    {
        List<string> AlllastMoves = new List<string>();
        
        foreach (var lastAction in m_lastActions)
        {
            AlllastMoves.Add(lastAction.ActionName);
        }

        if (recentNode != null)
        {
            List<string> nodeMoves = new List<string>();

            nodeMoves.Add(recentNode.Action.ActionName);

            FT_ActionNode parentNode = recentNode.ParentNode;

            while (parentNode != null)
            {
                nodeMoves.Add(parentNode.Action.ActionName);
                parentNode = parentNode.ParentNode;
            }

            for (int i = nodeMoves.Count; i > 0; i--)
            {
                AlllastMoves.Add(nodeMoves[i - 1]);
            }
        }

        string[] ComboSequence = FightUtil.GetAI().ComboHandler.GetComboSequence(comboName);
        
        if(AlllastMoves.Count < ComboSequence.Length) { return false; }

        List<string> relevantLastMoves = AlllastMoves.Count > ComboSequence.Length ?
                AlllastMoves.GetRange(AlllastMoves.Count - ComboSequence.Length, ComboSequence.Length) : AlllastMoves;

        for(int i = 0; i < ComboSequence.Length; i++)
        {
            if(ComboSequence[i] != relevantLastMoves[i]) { return false; }
        }

        return true;
    }
}


public class FT_CoolDownAction
{
    public string ActionName;
    public float Timer;

    public FT_CoolDownAction(string actionName, float timer)
    {
        ActionName = actionName;
        Timer = timer;
    }
}
}
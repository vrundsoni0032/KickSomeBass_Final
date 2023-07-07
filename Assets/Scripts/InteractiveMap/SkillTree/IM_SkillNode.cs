using UnityEngine;
using UnityEngine.Events;

namespace InteractiveMap.SkillTree
{
public enum NodeState { Locked, Unlocked, Obtained }

public class IM_SkillNode : MonoBehaviour
{
    public string Name;
    public Sprite Icon;
    public GameObject LockedIcon;
    public GameObject ObtainedIcon;

    public string Description;
    public int Cost;
    public NodeState State;
    public UnityEvent TraitAction;
    public IM_SkillNode[] ChildNodes;

    private void Start() => SetState(State);

    public void SetState(NodeState newState)
    {
        State = newState;

        switch (State)
        {
            case NodeState.Locked:
                LockedIcon.SetActive(true);
                ObtainedIcon.SetActive(false);
                break;
            case NodeState.Unlocked:
                LockedIcon.SetActive(false);
                ObtainedIcon.SetActive(false);
                break;
            case NodeState.Obtained:
                LockedIcon.SetActive(false);
                ObtainedIcon.SetActive(true);
                break;
        }
    }
}
}
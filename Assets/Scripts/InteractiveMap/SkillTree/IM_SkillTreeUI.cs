using Core;
using Core.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveMap.SkillTree
{ 
public class IM_SkillTreeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_skillPoints;
    [SerializeField] private TMP_Text m_skillName;
    [SerializeField] private TMP_Text m_skillDescription;
    [SerializeField] private GameObject m_CostObject;
    [SerializeField] private TMP_Text m_skillCost;

    [SerializeField] private Button m_upgradeButton;
    [SerializeField] private Color m_obtainableButtonColor;
    [SerializeField] private Color m_unobtainableButtonColor;


    private IM_SkillNode m_selectedNode;

    public void Start()
    {
        m_selectedNode = null;
        m_skillDescription.text = "Select Ability.";
        m_skillCost.text = 0.ToString();
        m_skillName.text = "";
        m_CostObject.SetActive(false);
        m_upgradeButton.gameObject.SetActive(false);
        m_skillPoints.text = GameUtil.PlayerState.SkillPoints.ToString();

        GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);
    }

    public void OnClickUpgrade()
    {
        if (m_selectedNode == null) { return; }

            GameUtil.SoundManager.PlaySound("ButtonPop");

            GameUtil.PlayerState.SkillPoints -= m_selectedNode.Cost;

        m_selectedNode.SetState(NodeState.Obtained);

        foreach (var childNode in m_selectedNode.ChildNodes)
        {
            if (childNode.State == NodeState.Locked)
            {
                childNode.SetState(NodeState.Unlocked);
            }
        }

        UpdateSkillTreeUI(m_selectedNode, true);
        m_selectedNode.TraitAction.Invoke();
    }

    public void OnSelect(IM_SkillNode node)
    {
        m_selectedNode = null;
        UpdateSkillTreeUI(node, false);

        if (node.State != NodeState.Unlocked) { return; }
        if (GameUtil.PlayerState.SkillPoints < node.Cost) { return; }

            GameUtil.SoundManager.PlaySound("ButtonPop");

            m_selectedNode = node;
        UpdateSkillTreeUI(node, true);

    }

    private void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent)
    {
        if(((IM_Core)GameUtil.MapCore).SkillTree.activeSelf == false) { return; }

        IM_Core.isAnyCanvasActive = false;
        ((IM_Core)GameUtil.MapCore).SkillTree.SetActive(false);
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
    }

    public void UpdateSkillTreeUI(IM_SkillNode node, bool enoughPoints)
    {
        m_skillPoints.text = GameUtil.PlayerState.SkillPoints.ToString();
        m_skillDescription.text = node.Description;

        if (node.State == NodeState.Obtained)
        {
            m_CostObject.SetActive(false);
            m_upgradeButton.gameObject.SetActive(false);
        }
        else
        {
            m_CostObject.SetActive(true);
            m_skillCost.text = node.Cost.ToString();

            m_upgradeButton.gameObject.SetActive(true);
            m_upgradeButton.GetComponent<Image>().color = enoughPoints ? m_obtainableButtonColor : m_unobtainableButtonColor;
        }

        m_skillName.text = node.Name;
    }

    public void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
        GameUtil.EventManager.UnRegisterSubscribers(HandleOverDrawnCanvasEvent);
}
}
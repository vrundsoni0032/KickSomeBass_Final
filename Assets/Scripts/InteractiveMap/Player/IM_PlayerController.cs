using Core;
using UnityEngine;
using Core.EventSystem;
using Core.Controller;
using InteractiveMap.Events;

namespace InteractiveMap.PlayerController
{
public class IM_PlayerController : KSB_IController
{
    private KSB_IEventManager m_eventManager;
    private IM_Player m_player;
    private Transform m_cameraTransform;

    public IM_PlayerController(IM_Player player, Transform cameraTransform)
    {
        m_eventManager = GameUtil.EventManager;
        m_eventManager.RegisterSubscriber(HandleInputEvent, KSB_InputEvent.EventID);

        m_player = player;
        m_cameraTransform = cameraTransform;
    }

    public void HandleInputEvent(KSB_IEvent gameEvent)
    {
        KSB_InputEvent inputEvent = (KSB_InputEvent)gameEvent;

        switch (inputEvent.Action)
        {
            case "Movement":
                Vector3 axisInput = inputEvent.AxisDirection;
                m_eventManager.AddEvent(new 
                    IM_MoveEvent(m_player, m_cameraTransform.forward * axisInput.z + m_cameraTransform.right * axisInput.x));
                break;

            case "IM_SkillTree":
                ToggleSkillTree(!((IM_Core)GameUtil.MapCore).SkillTree.activeSelf);
                break;

            case "IM_ToggleInventory":
                m_eventManager.AddEvent(new IM_ToggleInventoryEvent());
                break;

            case "IM_Interact":
                HandleInteraction();
                break;
        }
    }


    public void HandleInteraction()
    {
        LayerMask layerMask = ~LayerMask.NameToLayer("interactable");

        Collider[] overlapColliders =  Physics.OverlapSphere(m_player.transform.position, 1.0f, layerMask);

        foreach (var collider in overlapColliders)
        {
            if (collider.GetComponent<IM_Interactable>() != null)
            {
                if (collider.GetComponent<IM_IDriveable>() != null)
                {
                    m_player.GetComponent<IM_Player>().EquipVehicle(collider.GetComponent<IM_IDriveable>());
                }
                else
                {
                    collider.GetComponent<IM_Interactable>().TriggerInteraction();
                }
                return;
            }
        }
    }

    public void ToggleSkillTree(bool enable)
    {
        if(IM_Core.isAnyCanvasActive) { return; }
        ((IM_Core)GameUtil.MapCore).SkillTree.SetActive(true);
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
        
        IM_Core.isAnyCanvasActive = true;
    }

    public void UnRegisterToEventSystem() => m_eventManager.UnRegisterSubscribers(HandleInputEvent);
}
}
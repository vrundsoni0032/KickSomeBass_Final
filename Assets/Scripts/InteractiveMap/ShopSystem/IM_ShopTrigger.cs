using InteractiveMap.Events;
using UnityEngine;

public class IM_ShopTrigger : MonoBehaviour, IM_Interactable
{
    public void TriggerInteraction() => GameUtil.EventManager.AddEvent(new IM_ToggleShopEvent());
}
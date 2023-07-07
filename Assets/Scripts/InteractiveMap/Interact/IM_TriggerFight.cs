using InteractiveMap.Events;
using UnityEngine;
using Core;

public class IM_TriggerFight : MonoBehaviour, IM_Interactable
{
    public KSB_CoreData FightCoreData;

    public void TriggerInteraction()
    {
        GameUtil.EventManager.AddEvent(new IM_ToggleLoadoutEvent(FightCoreData));
    }
}

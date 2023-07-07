using System;
using UnityEngine;
using Core.EventSystem;
using ShopSystem;
using Core;

namespace InteractiveMap.Events
{
public class IM_MoveEvent : KSB_IEvent
{
    public readonly IM_Player Player;
    public readonly Vector3 Direction;

    public IM_MoveEvent(IM_Player player, Vector3 direction) { Player = player; Direction = direction; }
    
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Move Event"; }
}

public class IM_ToggleInventoryEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Open Ability Tree Event"; }
}

public class IM_ToggleShopEvent : KSB_IEvent
{
    public IM_ToggleShopEvent() { }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Toggle Shop Event"; }
}

public class IM_ToggleLoadoutEvent : KSB_IEvent
{
    public readonly KSB_CoreData CoreData;

    public IM_ToggleLoadoutEvent(KSB_CoreData  aCoreData) { CoreData = aCoreData; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Open Loadout Event"; }
}

public class IM_AddInventoryItemEvent : KSB_IEvent
{
    public readonly InventoryItem Item;
    public readonly int ItemPrice;
    public readonly bool bItemFromLoadout = false;

    public IM_AddInventoryItemEvent(InventoryItem aItem, int aPrice = 0, bool aFromloadout = false)
    {
        Item = aItem;
        ItemPrice = aPrice;
        bItemFromLoadout = aFromloadout;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Add Inventory Item Event"; }
}

public class IM_UpdateLoadOutScreenEvent : KSB_IEvent
{
    public IM_UpdateLoadOutScreenEvent() { }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Two);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Update LoadOut Screen Event"; }
}
}
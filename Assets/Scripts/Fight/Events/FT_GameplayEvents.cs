using System;
using Core.EventSystem;
using Fight.Fighter;
using UnityEngine;

namespace Fight.Events
{
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 //////////////////////////////// Gameplay Events //////////////////////////////////////////////////////////////////
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class FT_ChangeHealthEvent : KSB_IEvent
{
   public readonly FT_Fighter Fighter;
   public readonly float ChangeAmount;
    
    public FT_ChangeHealthEvent(FT_Fighter fighter, float changeAmount) { Fighter = fighter; ChangeAmount = changeAmount; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Change Health Event"; }
}

public class FT_ChangeStaminaEvent : KSB_IEvent
{
   public readonly FT_Fighter Fighter;
   public readonly float ChangeAmount;
    
    public FT_ChangeStaminaEvent(FT_Fighter fighter, float changeAmount) { Fighter = fighter; ChangeAmount = changeAmount; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Change Stamina Event"; }
}
public class FT_ItemUsedEvent : KSB_IEvent
{
   public readonly int ItemIndex;
   public readonly int ItemQuantity;
    
    public FT_ItemUsedEvent(int aItemIndex, int aItemQuantity) { ItemIndex = aItemIndex;ItemQuantity = aItemQuantity; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "ItemUsedEvent"; }
}

public class FT_ShowDamageUIEvent : KSB_IEvent
{
   public readonly FT_Fighter Fighter;
    public FT_ShowDamageUIEvent(FT_Fighter fighter) { Fighter = fighter; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Show Damage UI"; }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 //////////////////////////////// AI Gameplay Events ///////////////////////////////////////////////////////////////
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class FT_PlayerStateChangedEvent : KSB_IEvent
{
    public readonly string ActionName; 
    public FT_PlayerStateChangedEvent(string actionName) { ActionName = actionName; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Player State Changed Event"; }
}
}
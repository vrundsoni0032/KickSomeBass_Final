using System;
using UnityEngine;
using Core.EventSystem;
using Fight.AbilitySystem;
using Fight.Fighter;

namespace Fight.Events
{
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 //////////////////////////////// Player Input Events //////////////////////////////////////////////////////////////
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class FT_MoveEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly Vector3 Direction;

    public FT_MoveEvent(FT_Fighter fighter, Vector3 direction) { Direction = direction; Fighter = fighter; }
    

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; } 

    public string GetDebugName() { return "Move Event"; }

}

public class FT_JumpEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public FT_JumpEvent(FT_Fighter fighter) { Fighter = fighter; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Jump Event"; }
}

public class FT_FreezeFighterActionEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly bool bFreezeMovement;
    public readonly bool bFreezeJump;
    public readonly bool bFreezeAbility;
    public readonly bool bFreezeStaminaGain;
    public readonly bool bFreezeHitSensor;

    public FT_FreezeFighterActionEvent(FT_Fighter fighter, bool freezeMovement, bool freezeJump, bool freezeAbility, bool freezeStaminaGain) 
    {
        Fighter = fighter; 
        bFreezeMovement = freezeMovement;
        bFreezeJump = freezeJump;
        bFreezeAbility = freezeAbility;
        bFreezeStaminaGain = freezeStaminaGain;
    }
    public FT_FreezeFighterActionEvent(FT_Fighter fighter, bool freezeMovement, bool freezeJump, bool freezeAbility, bool freezeStaminaGain,bool freezeHitSensor) 
    {
        Fighter = fighter; 
        bFreezeMovement = freezeMovement;
        bFreezeJump = freezeJump;
        bFreezeAbility = freezeAbility;
        bFreezeStaminaGain = freezeStaminaGain;
        bFreezeHitSensor= freezeHitSensor;
    }
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Freeze Fighter Action Event"; }
}

public class FT_AbilityEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly string AbilityName;
    public readonly Vector3 AbilityDirection;
    public readonly FT_AbilityState AbilityState;

    public FT_AbilityEvent(FT_Fighter fighter, string abiltyName, Vector3 direction, FT_AbilityState abilityState = FT_AbilityState.Begin) 
    { 
        Fighter = fighter;
        AbilityName = abiltyName;
        AbilityDirection = direction;
        AbilityState = abilityState;
    }
    
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Ability Event"; }
}

public class FT_AbilitySucessEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly string AbilityName;

    public FT_AbilitySucessEvent(FT_Fighter fighter, string abiltyName) { Fighter = fighter; AbilityName = abiltyName; }
    
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "AbilitySucessEvent"; }
}

public class FT_UseItemEvent : KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly int ItemIndex;
    public readonly Vector3 AbilityDirection;
    public readonly FT_AbilityState AbilityState;

    public FT_UseItemEvent(FT_Fighter fighter, int itemIndex) 
    { 
        Fighter = fighter;
        ItemIndex = itemIndex;
    }
    
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Ability Event"; }
}

public class FT_PlayAnimationEvent: KSB_IEvent
{
    public readonly FT_Fighter Fighter;
    public readonly string AnimationAction;
    public readonly Action<string> KeyFrameCallback;
    public readonly Action<string> AnimationEndCallback;

    public FT_PlayAnimationEvent(FT_Fighter fighter, string animationAction)
    {
        Fighter = fighter;
        AnimationAction = animationAction;
    }
    public FT_PlayAnimationEvent(FT_Fighter fighter, string animationAction,Action<string> keyFrameCallback)
    {
        Fighter = fighter;
        AnimationAction = animationAction;
        KeyFrameCallback = keyFrameCallback;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "PlayAnimationEvent"; }
}
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 //////////////////////////////// AI Input Events //////////////////////////////////////////////////////////////////
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
public class FT_AIInputEvent: KSB_IEvent
{
    public readonly string Action;
    public FT_AIInputEvent(string action) { Action = action; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "AI Input Event"; }
}
}
using System;
using Core.EventSystem;

namespace GameInterface.Events
{
public class GI_EndTitleScreenEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Five); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "End Title Screen Event"; }
}

public class GI_InstantiateUIPrefabEvent : KSB_IEvent
{
    public readonly string PrefabToInstantiateLiteral;

    public GI_InstantiateUIPrefabEvent(string prefabToInstantiatePath) { PrefabToInstantiateLiteral = prefabToInstantiatePath; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Five); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Instantiate UI Prefab Event"; }
}

public class GI_DestroyCurrentUIPrefabEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Five);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Destroy Current UI Prefab Event"; }
}
}
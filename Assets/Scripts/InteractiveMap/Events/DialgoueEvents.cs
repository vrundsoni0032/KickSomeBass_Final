using System;
using Core.EventSystem;
using UnityEngine;

namespace InteractiveMap.DialogueSystem
{
public class IM_ChangeDialogueEvent : KSB_IEvent
{
    public readonly string Name;
    public readonly Sprite Image;
    public readonly Dialogue Dialogue;

    public IM_ChangeDialogueEvent(string name, Sprite image, Dialogue dialogue)
    {
        Name = name;
        Image = image;
        Dialogue = dialogue;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Change dialogue Event"; }
}

public class IM_ToggleDialogueBoxEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Toggle Dialogue Box Event"; }
}
}
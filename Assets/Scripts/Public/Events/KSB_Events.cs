using System;
using UnityEngine;
using Core.InputSystem;

namespace Core.EventSystem
{
public class KSB_NullEvent : KSB_IEvent
{ 
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "Null Event"; }
}

public class KSB_EndSceneEvent : KSB_IEvent
{
    public readonly string SceneToLoadName;
    public readonly KSB_ICore SceneToLoadCore;
    public readonly KSB_CoreData CoreData;

    public KSB_EndSceneEvent(string sceneToLoadName,KSB_ICore sceneToLoadCore, KSB_CoreData coreData)
    {
        SceneToLoadName = sceneToLoadName;
        SceneToLoadCore = sceneToLoadCore;
        CoreData = coreData;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "End Scene Event"; }

}

public class KSB_InputEvent : KSB_IEvent
{
    public readonly Vector3 AxisDirection;
    public readonly string Action;
    public readonly KSB_InputKeyState KeyState;

    public KSB_InputEvent(string action, KSB_InputKeyState keyState) { Action = action; KeyState = keyState; }
    public KSB_InputEvent(string action,Vector3 aAxisDirection) { Action = action; AxisDirection = aAxisDirection; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "Input Event"; }
}

public class KSB_ClearTimerEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "Clear Timer Event"; }
}

public class KSB_FirstSceneToLoadEvent : KSB_IEvent
{
    public readonly string SceneToLoadName;
    public readonly Core.KSB_ICore SceneToLoadCore;
    public readonly bool LoadNewGame;

    public KSB_FirstSceneToLoadEvent(string sceneToLoadName, Core.KSB_ICore sceneToLoadCore, bool loadNewGame)
    {
        SceneToLoadName = sceneToLoadName;
        SceneToLoadCore = sceneToLoadCore;
        LoadNewGame = loadNewGame;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "First Scene To Load Event"; }
}

public class KSB_GameQuitEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 

    public string GetDebugName() { return "Game Quit Event"; }
}

public class KSB_EscapeEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "Escape Event"; }

}

public class KSB_PauseEvent : KSB_IEvent
{
    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Pause Event"; }
    }

public class KSB_InterruptGameplayInputEvent : KSB_IEvent
{
    public readonly bool InterruptGameplayInput = false;
    public KSB_InterruptGameplayInputEvent(bool interruptGameplayInput) { InterruptGameplayInput = interruptGameplayInput; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One); 
    public UInt64 GetEventID() { return EventID; } 
    public string GetDebugName() { return "Interrupt Input Event"; }
}

public class KSB_MouseStateEvent : KSB_IEvent
{
    public readonly bool Visibility = false;
    public readonly CursorLockMode CursorState;

    public KSB_MouseStateEvent(bool visiblity, CursorLockMode cursorState = CursorLockMode.Confined)
    {
        Visibility = visiblity;
        CursorState = cursorState;
    }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.One);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Interrupt Input Event"; }
}
}
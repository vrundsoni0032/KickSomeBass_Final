using UnityEngine;

namespace Core.InputSystem
{
public enum KSB_InputKeyState
{
    None,
    KeyPressedOnce,
    KeyPressed,
    KeyUp,
    KeboardAxis,
    MouseAxis,
};

public enum KSB_InputDeviceType { Null, KeyboardMouse, Gamepad };
public enum KSB_InputPhase { Null, InteractiveMap, Fight, Persistent };

[System.Serializable]
public struct KSB_Input
{
    [Tooltip("Event System will notify for the input event holding the action passed here.")]
    public string Action;
    public KeyCode Keycode;
    public KSB_InputKeyState InputKeyState;
}


[CreateAssetMenu(fileName = "NewKeyBindings_DeviceType", menuName = "KSBUtilities/Input/KeyBindings")]
public class KSB_SO_KeyBindings : ScriptableObject
{
    public KSB_InputDeviceType InputDeviceType;
    public KSB_InputPhase InputPhase;
    public KSB_Input[] Input;
}
}
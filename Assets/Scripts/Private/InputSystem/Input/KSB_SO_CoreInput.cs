using UnityEngine;
using Core.EventSystem;
using Core.InputSystem;

namespace PrivateCore.InputSystem
{
[CreateAssetMenu(fileName = "KSB_CoreInput", menuName = "KSBUtilities/Input/CoreInput")]
public class KSB_SO_CoreInput : ScriptableObject
{
    [SerializeField]private KSB_SO_KeyBindings KSB_KeyboardMousePersistentInput;
    [SerializeField]private KSB_SO_KeyBindings KSB_GamepadPersistentInput;

    [SerializeField]private KSB_SO_KeyBindings IM_KeyboardMouseInput;
    [SerializeField]private KSB_SO_KeyBindings IM_GamepadInput;

    [SerializeField]private KSB_SO_KeyBindings FT_KeyboardMouseInput;
    [SerializeField]private KSB_SO_KeyBindings FT_GamepadInput;

    public void UpdateCoreInput(KSB_InputPhase inputPhase, KSB_InputDeviceType inputDeviceType)
    {
        KSB_SO_KeyBindings currentBindings = GetKeyBinds(inputPhase, inputDeviceType);


        foreach(KSB_Input input  in currentBindings.Input)
        {
            switch(input.InputKeyState)
            {
            case KSB_InputKeyState.KeyPressedOnce:
            
                if (Input.GetKeyDown(input.Keycode)) { BrodcastInputEvent(input.Action, KSB_InputKeyState.KeyPressedOnce); }
                break;
            
            case KSB_InputKeyState.KeyPressed:

                if (Input.GetKey(input.Keycode)) { BrodcastInputEvent(input.Action, KSB_InputKeyState.KeyPressed); }
                break;
    
            case KSB_InputKeyState.KeyUp:

                if (Input.GetKeyUp(input.Keycode)) { BrodcastInputEvent(input.Action, KSB_InputKeyState.KeyUp); }
                break;

            case KSB_InputKeyState.KeboardAxis:
                Vector3 keyboardInput = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
            
                if(keyboardInput != Vector3.zero) { BrodcastAxisInputEvent(input.Action,keyboardInput);}
                break;
            case KSB_InputKeyState.MouseAxis:
                Vector3 mouseInput = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

                if(mouseInput != Vector3.zero) 
                {
                    BrodcastAxisInputEvent(input.Action, mouseInput);
                }
                break;
            }
        }
    }

    private void BrodcastInputEvent(string action, KSB_InputKeyState keyState) { GameUtil.EventManager.AddEvent(new KSB_InputEvent(action, keyState));}
    private void BrodcastAxisInputEvent(string action,Vector3 aAxisValue) { GameUtil.EventManager.AddEvent(new KSB_InputEvent(action,aAxisValue));}

    private KSB_SO_KeyBindings GetKeyBinds(KSB_InputPhase inputPhase, KSB_InputDeviceType inputDeviceType)
    {
        switch(inputPhase)
        {
            case KSB_InputPhase.Persistent:

                if(inputDeviceType == KSB_InputDeviceType.KeyboardMouse) { return KSB_KeyboardMousePersistentInput; }
                else if(inputDeviceType == KSB_InputDeviceType.Gamepad) { return KSB_GamepadPersistentInput; }
                break;  

            case KSB_InputPhase.InteractiveMap:

                if(inputDeviceType == KSB_InputDeviceType.KeyboardMouse) { return IM_KeyboardMouseInput; }
                else if(inputDeviceType == KSB_InputDeviceType.Gamepad) { return IM_GamepadInput; }
                break;  

            case KSB_InputPhase.Fight:
                if(inputDeviceType == KSB_InputDeviceType.KeyboardMouse) { return FT_KeyboardMouseInput; }
                else if(inputDeviceType == KSB_InputDeviceType.Gamepad) { return FT_GamepadInput; }
                break;
        }

        YCLogger.Assert("CoreInput", false, "KeyBindings Not Found");
        return null;
    }
}
}
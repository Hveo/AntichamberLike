using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.iOS;

public static class InputHandler
{
    //public Dictionary<string, InputAction> Inputs;
    public static PlayerInput Inputs;
    public static bool PCLayout;

    public static string DeviceName { get; private set; }

    public delegate void onInputDeviceChanged();
    public static onInputDeviceChanged onInputDeviceChangedDelegate; 

    public struct InputBindingInfo
    {
        public InputAction Action;
        public int BindIndex;
        public string ControlPath;
        public bool IsComposite;

        public InputBindingInfo(InputAction act, int index, string path, bool isComposite)
        {
            Action = act;
            BindIndex = index;
            ControlPath = path;
            IsComposite = isComposite;
        }
    }

    public static void InitiateInput()
    {
        Inputs = GameObject.FindObjectOfType<PlayerInput>();
        GameObject.DontDestroyOnLoad(Inputs.gameObject);

        LoadInputPrefs();
        InputUser.onChange += OnInputDeviceChange;
    }

    public static InputBindingInfo[] GetInputsForCurrentDevice()
    {
        var InputActions = Inputs.actions.actionMaps[0].actions;
        List<InputBindingInfo> InputBindInfos = new List<InputBindingInfo>();

        for (int i = 0; i < InputActions.Count; ++i)
        {
            for (int j = 0; j < InputActions[i].bindings.Count; ++j)
            {
                InputActions[i].GetBindingDisplayString(j, out var DeviceLayoutName, out var ControlPath, InputBinding.DisplayStringOptions.DontIncludeInteractions);

                if (string.IsNullOrEmpty(DeviceLayoutName) || (string.CompareOrdinal(DeviceLayoutName, "Gamepad") == 0 && PCLayout) || (IsPCDevice(DeviceLayoutName) && !PCLayout))
                    continue;

                InputBinding bind = InputActions[i].bindings[j];
                InputBindInfos.Add(new InputBindingInfo(InputActions[i], j, bind.effectivePath, bind.isPartOfComposite));
            }
        }

        return InputBindInfos.ToArray();
    }

    static void SetDeviceName(string Layout)
    {
        if (IsPCDevice(Layout))
            DeviceName = "Mouse And Keyboard";
        else if (Layout.Contains("XInput"))
            DeviceName = "Xbox Controller";
        else if (Layout.Contains("Dual"))
            DeviceName = "PlayStation Controller";
        else if (Layout.Contains("Switch"))
            DeviceName = "Nintendo Switch Pro Controller";
        else
            DeviceName = Layout;
    }

    static void LoadInputPrefs()
    {
        /*Read file and map controls*/

    }

    public static void EnableInputs()
    {
        Inputs.ActivateInput();
    }

    public static void DisableInputs()
    {
        Inputs.DeactivateInput();
    }

    static void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (device is null || change != InputUserChange.DevicePaired)
            return;

        SetDeviceName(device.name);

        if (IsPCDevice(device.name))
        {            
            if (!PCLayout) //No need to call unnecessary events if the layout still the same
            {
                PCLayout = true;
                onInputDeviceChangedDelegate?.Invoke();
            }

            return;
        }
        
        PCLayout = false;
        onInputDeviceChangedDelegate?.Invoke();
    }
    
    public static bool IsPCDevice(string name)
    {
        return (string.CompareOrdinal(name, "Mouse") == 0 || string.CompareOrdinal(name, "Keyboard") == 0);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.iOS;

/// <summary>
/// A classe used for serialization purpose because JSON won't serialize a local list
/// </summary>
[Serializable]
public class InputWrapper
{
    public List<InputHandler.InputOverrides> Overrides;
}

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

    [System.Serializable]
    public struct InputOverrides
    {
        public string guid;
        public string overridePath;
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
        string inputPath = Core.instance.DataPath + "/InputPrefs.json";
        InputWrapper wrapper = new InputWrapper(); 

        if (File.Exists(inputPath))
        {
            string Json = File.ReadAllText(inputPath);
            JsonUtility.FromJsonOverwrite(Json, wrapper);
        }
        else
            return;

        /*Read file and map controls*/      
        foreach (var map in Inputs.actions.actionMaps)
        {
            var bindings = map.bindings;
            for (var i = 0; i < bindings.Count; ++i)
            {
                for (int j = 0; j < wrapper.Overrides.Count; ++j)
                {
                    if (string.CompareOrdinal(wrapper.Overrides[j].guid, bindings[i].id.ToString()) == 0)
                    {
                        map.ApplyBindingOverride(i, new InputBinding { overridePath = wrapper.Overrides[j].overridePath });
                        break;
                    }
                }
            }
        }
    }

    public static void SaveInputPrefs()
    {
        InputWrapper inputClass = new InputWrapper();
        inputClass.Overrides = new List<InputOverrides>();

        foreach (var map in Inputs.actions.actionMaps)
        {
            foreach (var binding in map.bindings)
            {
                if (!string.IsNullOrEmpty(binding.overridePath))
                {
                    inputClass.Overrides.Add(new InputOverrides { guid = binding.id.ToString(), overridePath = binding.overridePath });
                }
            }
        }
     
        string Json = JsonUtility.ToJson(inputClass);
        File.WriteAllText(Core.instance.DataPath + "/InputPrefs.json", Json);
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

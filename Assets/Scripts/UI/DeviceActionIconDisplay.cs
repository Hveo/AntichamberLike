using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceActionIconDisplay : MonoBehaviour
{
    public InputActionReference Action;

    private Image m_InputImage;

    private void OnEnable()
    {
        m_InputImage = GetComponent<Image>();

        if (m_InputImage is null)
        {
            Destroy(this);
            return;
        }

        InputHandler.onInputDeviceChangedDelegate += ChangeLayout;
        ChangeLayout();
    }


    void ChangeLayout()
    {
        string InputPath = string.Empty;

        for (int i = 0; i < Action.action.bindings.Count; ++i)
        {
            Action.action.GetBindingDisplayString(i, out var DeviceLayoutName, out var controlPath);

            if (string.IsNullOrEmpty(DeviceLayoutName) || (string.CompareOrdinal(DeviceLayoutName, "Gamepad") == 0 && InputHandler.PCLayout) || (InputHandler.IsPCDevice(DeviceLayoutName) && !InputHandler.PCLayout))
                continue;

            InputPath = Action.action.bindings[i].effectivePath;
        }

        InputPath = InputPath.Substring(InputPath.LastIndexOf("/") + 1);

        Sprite inputIcon = Resources.Load<Sprite>("Inputs/" + InputHandler.DeviceName + "/" + InputPath);

        if (inputIcon is null)
            return;

        m_InputImage.sprite = inputIcon;
    }

    private void OnDisable()
    {
        InputHandler.onInputDeviceChangedDelegate -= ChangeLayout;
    }

    private void OnDestroy()
    {
        InputHandler.onInputDeviceChangedDelegate -= ChangeLayout;
    }
}

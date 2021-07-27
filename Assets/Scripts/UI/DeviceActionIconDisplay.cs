using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceActionIconDisplay : MonoBehaviour
{
    public InputActionReference Action;
    public bool KeepCurrentImageForPC;

    private Image m_InputImage;
    private Sprite m_PCSprite;

    private void OnEnable()
    {
        m_InputImage = GetComponent<Image>();

        if (m_InputImage is null)
        {
            Destroy(this);
            return;
        }

        if (KeepCurrentImageForPC)
            m_PCSprite = m_InputImage.sprite;

        InputHandler.onInputDeviceChangedDelegate += ChangeLayout;
        ChangeLayout();
    }


    void ChangeLayout()
    {
        if (InputHandler.PCLayout && KeepCurrentImageForPC)
        {
            m_InputImage.sprite = m_PCSprite;
            return;
        }

        string InputPath = string.Empty;

        for (int i = 0; i < Action.action.bindings.Count; ++i)
        {
            Action.action.GetBindingDisplayString(i, out var DeviceLayoutName, out var controlPath);

            if (string.IsNullOrEmpty(DeviceLayoutName) || (!InputHandler.IsPCDevice(DeviceLayoutName) && InputHandler.PCLayout) || (InputHandler.IsPCDevice(DeviceLayoutName) && !InputHandler.PCLayout))
                continue;

            InputPath = Action.action.bindings[i].effectivePath;
        }

        InputPath = InputPath.Substring(InputPath.LastIndexOf("/") + 1);

        Sprite inputIcon = Resources.Load<Sprite>("Inputs/" + InputHandler.DeviceName + "/" + InputPath);

        if (inputIcon is null)
        {
            m_InputImage.color = Color.clear;
            return;
        }

        m_InputImage.sprite = inputIcon;
        m_InputImage.color = Color.white;
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

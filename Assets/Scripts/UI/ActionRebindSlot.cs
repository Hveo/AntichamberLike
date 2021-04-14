using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ActionRebindSlot : MonoBehaviour
{
    public TextMeshProUGUI ActionLabel;
    public string ID;

    public string ActionPath;
    public UnityEngine.UI.Button ActionRebindButton;

    public string AltPath;
    public UnityEngine.UI.Button AltRebindButton;

    public int BindIndex { get; private set; } = -1;
    public int AltIndex { get; private set; } = -1;

    private void Start()
    {
        AltRebindButton.gameObject.SetActive(InputHandler.PCLayout);
    }

    public void SetIndex(int index)
    {
        if (BindIndex == -1)
        {
            BindIndex = index;
            return;
        }

        if (InputHandler.PCLayout && AltIndex == -1)
        {
            AltIndex = index;
            return;
        }
    }

    public void LockBind()
    {
        ActionRebindButton.interactable = false;
        AltRebindButton.interactable = false;
    }

    public void SetInputIcon()
    {
        string key = ActionPath.Substring(ActionPath.LastIndexOf("/") + 1);

        if (key.Contains("Trigger"))
        {
            int index = key.LastIndexOf("Button");

            if (index != -1)
                key = key.Substring(0, index);
        }

        string path = "Inputs/" + InputHandler.DeviceName + "/" + key;
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite is null)
            return;

        ActionRebindButton.image.sprite = sprite;
    }

    public void SetAltIcon()
    {
        string key = AltPath.Substring(AltPath.LastIndexOf("/") + 1);
        string path = "Inputs/" + InputHandler.DeviceName + "/" + key;

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite is null)
            return;

        AltRebindButton.image.sprite = sprite;
    }
}

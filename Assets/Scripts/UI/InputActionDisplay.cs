using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputActionDisplay : MonoBehaviour
{
    public string ActionName;
    Image m_CurrentImg;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentImg = GetComponent<Image>();
        InputHandler.onInputDeviceChangedDelegate += HotSwapImage;
    }

    void HotSwapImage()
    {
        m_CurrentImg.sprite = InputHandler.GetIconForAction(ActionName);
    }

    private void OnDestroy()
    {
        InputHandler.onInputDeviceChangedDelegate -= HotSwapImage;
    }

}

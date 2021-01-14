using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public UnityEngine.UI.Button[] Buttons;

    public void EnableInput()
    {
        for (int i = 0; i < Buttons.Length; ++i)
        {
            Buttons[i].enabled = true;
        }
    }

    public void Exit()
    {
        Debug.Log("MDR");
        Application.Quit();
    }
}

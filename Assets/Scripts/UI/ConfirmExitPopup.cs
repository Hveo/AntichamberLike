using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmExitPopup : MonoBehaviour
{
    public void Cancel()
    {
        UISystem.instance.CloseWindow(gameObject);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

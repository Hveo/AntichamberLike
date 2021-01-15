using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void EnableInput()
    {
        UISystem.instance.ToggleWindowInteractable(gameObject, true);
    }

    public void OpenConfirmPopup()
    {
        UISystem.instance.ToggleConfirmExit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

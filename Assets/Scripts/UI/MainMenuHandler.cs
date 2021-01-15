using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    GameObject m_AboutWindow;
    IEnumerator Start()
    {
        ResourceRequest req = Resources.LoadAsync("AboutWindow");

        while (!req.isDone)
            yield return null;

        m_AboutWindow = req.asset as GameObject;
    }

    public void EnableInput()
    {
        UISystem.instance.ToggleWindowInteractable(gameObject, true);
    }

    public void OpenConfirmPopup()
    {
        UISystem.instance.ToggleConfirmExit();
    }

    public void OpenAboutPage()
    {
        if (m_AboutWindow != null)
        {
            UISystem.instance.NewFocusedWindow(GameObject.Instantiate(m_AboutWindow), true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

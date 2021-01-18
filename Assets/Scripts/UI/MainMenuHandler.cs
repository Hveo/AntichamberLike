using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour, IUIWindows
{
    GameObject m_AboutWindow;
    GameObject m_SettingsWindow;
    IEnumerator Start()
    {
        ResourceRequest req = Resources.LoadAsync("AboutWindow");

        while (!req.isDone)
            yield return null;

        m_AboutWindow = req.asset as GameObject;

        req = Resources.LoadAsync("SettingsWindow");

        while (!req.isDone)
            yield return null;

        m_SettingsWindow = req.asset as GameObject;
    }

    public void EnableInput()
    {
        UISystem.instance.NewFocusedWindow(gameObject);
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

    public void OpenSettingsWindow()
    {
        if (m_SettingsWindow != null)
        {
            UISystem.instance.NewFocusedWindow(GameObject.Instantiate(m_SettingsWindow), true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(transform.GetChild(0).gameObject);
    }

    public void FeedUIElementsWithEvents()
    {

    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }
}

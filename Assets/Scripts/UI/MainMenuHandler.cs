using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuHandler : MonoBehaviour, IUIWindows
{
    public Selectable[] Selectables;

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

    public bool IsPersistant()
    {
        return true;
    }

    public void EnableInput()
    {
        //UISystem.instance.NewFocusedWindow(gameObject);
        FeedUIElementsWithEvents();
        UISystem.instance.SetMenuPresence(true);
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
        UISystem.instance.SetMenuPresence(false);
        SceneManager.LoadScene(1);
    }

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(transform.GetChild(0).gameObject);
    }

    public void FeedUIElementsWithEvents()
    {
        for (int i = 0; i < Selectables.Length; ++i)
        {
            EventTrigger trigg = Selectables[i].GetComponent<EventTrigger>();

            if (trigg != null)
            {
                GameObject obj = Selectables[i].gameObject;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnSelect(PointerEventData);
                });

                trigg.triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.Select;
                entry2.callback.AddListener((PointerEventData) =>
                {
                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
                    {
                        UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
                        AudioMgr.PlayUISound("Select");
                    }
                });

                trigg.triggers.Add(entry2);

                EventTrigger.Entry entry3 = new EventTrigger.Entry();
                entry3.eventID = EventTriggerType.PointerExit;
                entry3.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnDeselect(PointerEventData);
                });

                trigg.triggers.Add(entry3);

                EventTrigger.Entry entry4 = new EventTrigger.Entry();
                entry4.eventID = EventTriggerType.Deselect;
                entry4.callback.AddListener((PointerEventData) =>
                {
                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
                        UIGraphicUtilities.DeselectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry4);
            }
        }
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }
}

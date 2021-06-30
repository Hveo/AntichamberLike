using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuWindow : MonoBehaviour, IUIWindows
{
    public Selectable[] Selectables;
    GameObject m_SettingsWindow;

    void Awake()
    {
        UISystem.instance.SetMenuPresence(true);
        AudioMgr.SetLowpassValue(500);
        AudioMgr.SetSFXVolume((int)(Core.instance.PlayerPrefs.FXVolume / 2));
        FeedUIElementsWithEvents();
    }

    IEnumerator Start()
    {
        ResourceRequest req = Resources.LoadAsync("SettingsWindow");
        
        while (!req.isDone)
            yield return null;

        m_SettingsWindow = req.asset as GameObject;
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

    public void Resume()
    {
        CloseWindow();
    }

    public void OpenOptionWindow()
    {
        if (m_SettingsWindow != null)
        {
            AudioMgr.PlayUISound("Validate");
            UISystem.instance.NewFocusedWindow(GameObject.Instantiate(m_SettingsWindow), true);
        }
    }

    public void BackToMainMenu()
    {
        UISystem.instance.CreatePopup(LocalizationSystem.GetEntry("menu.confirmmain"), "menu.yes", "menu.no",
            () =>
            {
                UISystem.instance.ClearAll();
                UISystem.instance.LockUnlockPauseAction(false);
                SceneManager.LoadScene(0);
            },
            () =>
            {
                UISystem.instance.CloseCurrentWindow();
            });

    }

    public void ExitGame()
    {
        UISystem.instance.ToggleConfirmExit();
    }

    GameObject IUIWindows.GetWindowObject()
    {
        return gameObject;
    }

    void IUIWindows.OnCancelInputPressed()
    {
        
    }

    private void CloseWindow()
    {
        AudioMgr.PlayUISound("Cancel");
        AudioMgr.SetLowpassValue(5000);
        AudioMgr.SetSFXVolume((int)Core.instance.PlayerPrefs.FXVolume);
        UISystem.instance.CloseCurrentWindow();
        UISystem.instance.ClearAll();
        Resources.UnloadUnusedAssets();
    }

    void IUIWindows.SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(Selectables[0].gameObject);
    }
}

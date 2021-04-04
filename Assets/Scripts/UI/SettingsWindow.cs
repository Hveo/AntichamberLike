using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsWindow : MonoBehaviour, IUIWindows
{
    bool m_ValueChanged;

    public Selectable[] Selectables;

    private GameObject m_RebindWindow;
    private TextMeshProUGUI m_SFXValue;
    private TextMeshProUGUI m_MusicValue;

    void Awake()
    {
        FeedUIElementsWithEvents();
    }

    IEnumerator Start()
    {
        ResourceRequest req = Resources.LoadAsync("BindingWindow");

        while (!req.isDone)
            yield return null;

        m_RebindWindow = req.asset as GameObject;
    }

    public bool IsPersistant()
    {
        return false;
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
                        UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
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

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(Selectables[0].gameObject);
    }

    public void OpenRebindWindow()
    {
        UISystem.instance.NewFocusedWindow(GameObject.Instantiate(m_RebindWindow), true);
    }

    public void ChangeLanguage(int language)
    {
        LocalizationSystem.ChangeLanguage((Language)language);
    }

    public void Cancel()
    {
        if (!m_ValueChanged)
            UISystem.instance.CloseWindow(gameObject);
    }

    public void SetMusicVolume(Slider slider)
    {
        AudioMgr.SetMusicVolume((int)slider.value);

        if (m_MusicValue == null)
            m_MusicValue = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        m_MusicValue.text = slider.value.ToString();
    }

    public void SetSFXVolume(Slider slider)
    {
        AudioMgr.SetSFXVolume((int)slider.value);
        
        if (m_SFXValue == null)
            m_SFXValue = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        m_SFXValue.text = slider.value.ToString();
    }
}

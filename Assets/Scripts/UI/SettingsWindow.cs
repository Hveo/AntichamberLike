using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsWindow : MonoBehaviour, IUIWindows
{
    public Selectable[] Selectables;

    private GameObject m_RebindWindow;
    private TextMeshProUGUI m_SFXValue;
    private TextMeshProUGUI m_MusicValue;
    private TextMeshProUGUI m_UIValue;
    private TextMeshProUGUI m_MouseSensitivity;
    private TextMeshProUGUI m_StickSensitivity;
    private PlayerPrefsObject m_TMPPref;

    void Awake()
    {
        FeedUIElementsWithEvents();
        m_TMPPref = Core.instance.PlayerPrefs.GetDeepCopy();
        LoadPrefs();
    }

    void LoadPrefs()
    {
        Selectables[0].GetComponent<Slider>().value = m_TMPPref.MusicVolume;
        Selectables[1].GetComponent<Slider>().value = m_TMPPref.FXVolume;
        Selectables[12].GetComponent<Slider>().value = m_TMPPref.UIVolume;

        Slider Mouse = Selectables[6].GetComponent<Slider>();
        Mouse.value = m_TMPPref.MouseSensitivity;
        m_MouseSensitivity = Mouse.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_MouseSensitivity.text = Mouse.value.ToString();

        Slider Stick = Selectables[7].GetComponent<Slider>();
        Stick.value = (m_TMPPref.StickSensitivity - 100.0f) / 5.0f;
        m_StickSensitivity = Stick.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_StickSensitivity.text = Stick.value.ToString();

        Selectables[8].GetComponent<Toggle>().isOn = m_TMPPref.InvertXAxis;
        Selectables[9].GetComponent<Toggle>().isOn = m_TMPPref.InvertYAxis;

        LocalizationSystem.ChangeLanguage(m_TMPPref.CurrentLanguage);
    }

    IEnumerator Start()
    {
        ResourceRequest req = Resources.LoadAsync("BindingWindow");

        while (!req.isDone)
            yield return null;

        m_RebindWindow = req.asset as GameObject;
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
                    else if (obj.GetComponent<Slider>() != null)
                        UIGraphicUtilities.SelectSlider(obj.GetComponent<Slider>());
                    else if (obj.GetComponent<Toggle>() != null)
                        UIGraphicUtilities.SelectToggle(obj.GetComponent<Toggle>());
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
                    else if (obj.GetComponent<Slider>() != null)
                        UIGraphicUtilities.DeselectSlider(obj.GetComponent<Slider>());
                    else if (obj.GetComponent<Toggle>() != null)
                        UIGraphicUtilities.DeselectToggle(obj.GetComponent<Toggle>());
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

    public void OnCancelInputPressed()
    {
        Cancel();
    }

    public void OpenRebindWindow()
    {
        UISystem.instance.NewFocusedWindow(GameObject.Instantiate(m_RebindWindow), true);
    }

    public void ChangeLanguage(int language)
    {
        m_TMPPref.CurrentLanguage = (Language)language;
        LocalizationSystem.ChangeLanguage((Language)language);
    }

    public void ResetDefaultPreset()
    {
        m_TMPPref.SetDefaultValue();
        LoadPrefs();
    }

    public void Apply()
    {
        Core.instance.PlayerPrefs = m_TMPPref;
        Core.instance.SavePlayerPrefs();
    }

    public void Cancel()
    {
        if (ValueChanged())
        {
            UISystem.instance.CreatePopup("settings.valuechanged", "menu.yes", "menu.no",
                () =>
                {
                    Apply();
                    UISystem.instance.CloseCurrentWindow();
                    UISystem.instance.CloseWindow(gameObject);
                },
                () =>
                {
                    if (m_TMPPref.CurrentLanguage != Core.instance.PlayerPrefs.CurrentLanguage)
                        LocalizationSystem.ChangeLanguage(Core.instance.PlayerPrefs.CurrentLanguage);

                    UISystem.instance.CloseCurrentWindow();
                    UISystem.instance.CloseWindow(gameObject);
                    AudioMgr.PlayUISound("Cancel");
                });
        }
        else
        {
            UISystem.instance.CloseCurrentWindow();
            AudioMgr.PlayUISound("Cancel");
        }
    }

    bool ValueChanged()
    {
        PlayerPrefsObject pp = Core.instance.PlayerPrefs;
        return m_TMPPref.MouseSensitivity != pp.MouseSensitivity || m_TMPPref.StickSensitivity != pp.StickSensitivity || m_TMPPref.FXVolume != pp.FXVolume ||
                m_TMPPref.MusicVolume != pp.MusicVolume || m_TMPPref.UIVolume != pp.UIVolume || m_TMPPref.InvertXAxis != pp.InvertXAxis || m_TMPPref.InvertYAxis != pp.InvertYAxis || 
                m_TMPPref.CurrentLanguage != pp.CurrentLanguage;
    }

    public void SetMusicVolume(Slider slider)
    {
        AudioMgr.SetMusicVolume((int)slider.value);

        if (m_MusicValue is null)
            m_MusicValue = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        m_MusicValue.text = slider.value.ToString();
        m_TMPPref.MusicVolume = slider.value;
    }

    public void SetSFXVolume(Slider slider)
    {
        AudioMgr.SetSFXVolume((int)slider.value);
        
        if (m_SFXValue is null)
            m_SFXValue = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        m_SFXValue.text = slider.value.ToString();
        m_TMPPref.FXVolume = slider.value;
    }

    public void SetUIVolume(Slider slider)
    {
        AudioMgr.SetUIVolume((int)slider.value);

        if (m_UIValue is null)
            m_UIValue = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        m_UIValue.text = slider.value.ToString();
        m_TMPPref.UIVolume = slider.value;
    }

    public void SetMouseSensitivity(Slider slider)
    {
        m_TMPPref.MouseSensitivity = slider.value;

        if (m_MouseSensitivity is null)
            m_MouseSensitivity = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        m_MouseSensitivity.text = slider.value.ToString();
    }

    public void SetStickSensitivity(Slider slider)
    {
        m_TMPPref.StickSensitivity = (slider.value * 5.0f) + 100.0f;

        if (m_StickSensitivity is null)
            m_StickSensitivity = slider.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        m_StickSensitivity.text = slider.value.ToString();
    }

    public void InvertXAxis(Toggle toggle)
    {
        m_TMPPref.InvertXAxis = toggle.isOn;
    }

    public void InvertYAxis(Toggle toggle)
    {
        m_TMPPref.InvertYAxis = toggle.isOn;
    }
}

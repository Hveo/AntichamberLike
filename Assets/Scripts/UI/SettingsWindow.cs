using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, IUIWindows
{
    bool m_ValueChanged;

    public Selectable[] Selectables;

    private TextMeshProUGUI m_SFXValue;
    private TextMeshProUGUI m_MusicValue;

    void Awake()
    {
        FeedUIElementsWithEvents();
    }

    public void FeedUIElementsWithEvents()
    {
        
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }

    public void SetDefaultItemSelected()
    {
        
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

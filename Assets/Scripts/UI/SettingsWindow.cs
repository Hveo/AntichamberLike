using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SettingsWindow : MonoBehaviour, IUIWindows
{
    public Selectable[] Selectables;
    public TextMeshProUGUI ResolutionText;
    public TextMeshProUGUI FrameText;

    private GameObject m_RebindWindow;
    private TextMeshProUGUI m_SFXValue;
    private TextMeshProUGUI m_MusicValue;
    private TextMeshProUGUI m_UIValue;
    private TextMeshProUGUI m_MouseSensitivity;
    private TextMeshProUGUI m_StickSensitivity;
    private PlayerPrefsObject m_TMPPref;
    private InputAction m_MoveSliderAction;
    private Slider m_CurrentSlider;
    private bool m_IsSliderSelected;
    private Resolution[] m_AvailableResolutions;
    private int m_ResIndex;
    private string[] m_FrameLimiter;
    private int m_FrameIndex;

    void Awake()
    {
        m_MoveSliderAction = InputHandler.Inputs.actions["MoveSlider"];
        UISystem.instance.onSelectionChangeEvent += SetNewSelection;

        FeedUIElementsWithEvents();
        m_TMPPref = Core.instance.PlayerPrefs.GetDeepCopy();
        m_FrameLimiter = new string[] { "30", "60", "120", "menu.none" };
        LoadPrefs();
    }

    void SetNewSelection(GameObject obj)
    {
        if (!(m_CurrentSlider is null))
            m_CurrentSlider.transform.GetChild(2).gameObject.SetActive(false);

        m_CurrentSlider = obj.GetComponent<Slider>();

        if (m_CurrentSlider is null)
        {
            m_IsSliderSelected = false;
            m_MoveSliderAction.Disable();
            return;
        }

        m_MoveSliderAction.Enable();
        m_IsSliderSelected = true;
        m_CurrentSlider.transform.GetChild(2).gameObject.SetActive(true);
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

        m_ResIndex = 0;
        m_AvailableResolutions = Screen.resolutions;
        ResolutionText.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;

        for (int i = 0; i < m_AvailableResolutions.Length; ++i)
        {
            if (m_AvailableResolutions[i].width == Screen.currentResolution.width && m_AvailableResolutions[i].height == Screen.currentResolution.height)
            {
                m_ResIndex = i;
                ResolutionText.text = m_AvailableResolutions[i].width + "x" + m_AvailableResolutions[i].height;
                break;
            }
        }

        for (int i = 0; i < m_FrameLimiter.Length; ++i)
        {
            if (string.CompareOrdinal(m_FrameLimiter[i], m_TMPPref.FrameLimit) == 0)
            {
                m_FrameIndex = i;
                FrameText.text = i == m_FrameLimiter.Length - 1 ? LocalizationSystem.GetEntry(m_FrameLimiter[i]) : m_FrameLimiter[i];
                break;
            }
        }

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
                    {
                        AudioMgr.PlayUISound("Select");
                        UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
                    }
                    else if (obj.GetComponent<Slider>() != null)
                        UIGraphicUtilities.SelectSlider(obj.GetComponent<Slider>());
                    else if (obj.GetComponent<Toggle>() != null)
                    {
                        AudioMgr.PlayUISound("Select");
                        UIGraphicUtilities.SelectToggle(obj.GetComponent<Toggle>());
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
                    else if (obj.GetComponent<Slider>() != null)
                        UIGraphicUtilities.DeselectSlider(obj.GetComponent<Slider>());
                    else if (obj.GetComponent<Toggle>() != null)
                        UIGraphicUtilities.DeselectToggle(obj.GetComponent<Toggle>());
                });

                trigg.triggers.Add(entry4);
            }
        }

        GameObject mcs = Selectables[13].gameObject;
        (Selectables[13] as MultipleChoiceSelectable).FeedNextPressed((InputAction.CallbackContext ctx) => 
        { 
            if (mcs.GetComponent<MultipleChoiceSelectable>().IsSelected) 
                NextResolution(); 
        });
        (Selectables[13] as MultipleChoiceSelectable).FeedPreviousPressed((InputAction.CallbackContext ctx) => 
        {
            if (mcs.GetComponent<MultipleChoiceSelectable>().IsSelected)
                PreviousResolution(); 
        });

        GameObject mcs1 = Selectables[14].gameObject;
        (Selectables[14] as MultipleChoiceSelectable).FeedNextPressed((InputAction.CallbackContext ctx) => 
        {
            if (mcs1.GetComponent<MultipleChoiceSelectable>().IsSelected)
                NextFrameLimit(); 
        });
        (Selectables[14] as MultipleChoiceSelectable).FeedPreviousPressed((InputAction.CallbackContext ctx) => 
        {
            if (mcs1.GetComponent<MultipleChoiceSelectable>().IsSelected)
                PreviousFrameLimit(); 
        });
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }

    void FixedUpdate()
    {
        if (m_IsSliderSelected)
        {
            Vector2 moveValue = m_MoveSliderAction.ReadValue<Vector2>();
            m_CurrentSlider.value += moveValue.x;
        }
    }

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(Selectables[13].gameObject);
    }

    public void OnCancelInputPressed()
    {
        Cancel();
    }

    public void NextResolution()
    {
        AudioMgr.PlayUISound("Browse");
        m_ResIndex++;

        if (m_ResIndex >= m_AvailableResolutions.Length)
            m_ResIndex = 0;

        ResolutionText.text = m_AvailableResolutions[m_ResIndex].width + "x" + m_AvailableResolutions[m_ResIndex].height;
        m_TMPPref.Resolution = ResolutionText.text;
    }

    public void PreviousResolution()
    {
        AudioMgr.PlayUISound("Browse");
        m_ResIndex--;

        if (m_ResIndex < 0)
            m_ResIndex = m_AvailableResolutions.Length - 1;

        ResolutionText.text = m_AvailableResolutions[m_ResIndex].width + "x" + m_AvailableResolutions[m_ResIndex].height;
        m_TMPPref.Resolution = ResolutionText.text;
    }

    public void NextFrameLimit()
    {
        AudioMgr.PlayUISound("Browse");
        m_FrameIndex++;

        if (m_FrameIndex >= m_FrameLimiter.Length)
            m_FrameIndex = 0;

        FrameText.text = m_FrameIndex == m_FrameLimiter.Length - 1 ? LocalizationSystem.GetEntry(m_FrameLimiter[m_FrameIndex]) : m_FrameLimiter[m_FrameIndex];
        m_TMPPref.FrameLimit = m_FrameLimiter[m_FrameIndex];
    }

    public void PreviousFrameLimit()
    {
        AudioMgr.PlayUISound("Browse");
        m_FrameIndex--;

        if (m_FrameIndex < 0)
            m_FrameIndex = m_FrameLimiter.Length - 1;

        FrameText.text = m_FrameIndex == m_FrameLimiter.Length - 1 ? LocalizationSystem.GetEntry(m_FrameLimiter[m_FrameIndex]) : m_FrameLimiter[m_FrameIndex];
        m_TMPPref.FrameLimit = m_FrameLimiter[m_FrameIndex];
    }

    public void OpenRebindWindow()
    {
        AudioMgr.PlayUISound("Validate");
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

    public void OnClickApply()
    {
        if (!Apply())
        {
            AskConfirmationForResolution();
        }
    }

    public bool Apply()
    {
        AudioMgr.PlayUISound("Apply");

        if (m_TMPPref.Resolution != Core.instance.PlayerPrefs.Resolution)
        {
            int[] Res = m_TMPPref.ParseResolution();
            Screen.SetResolution(Res[0], Res[1], true);
            return false;
        }

        Core.instance.ApplyFrameLimit(m_TMPPref.FrameLimit);
        Core.instance.PlayerPrefs = m_TMPPref;
        Core.instance.SavePlayerPrefs();
        return true;
    }

    void AskConfirmationForResolution()
    {
        UISystem.instance.CreateTimedPopup(LocalizationSystem.GetEntry("settings.keepchange"), "menu.yes", "menu.no",
        () =>
        {
            Core.instance.PlayerPrefs = m_TMPPref;
            Core.instance.ApplyFrameLimit(m_TMPPref.FrameLimit);
            Core.instance.SavePlayerPrefs();
            UISystem.instance.CloseCurrentWindow();
            UISystem.instance.CloseWindow(gameObject);
        },
        () =>
        {
            int[] Res = Core.instance.PlayerPrefs.ParseResolution();
            Screen.SetResolution(Res[0], Res[1], true);
            UISystem.instance.CloseCurrentWindow();
        }, 5.0f);
    }

    public void Cancel()
    {
        if (ValueChanged())
        {
            UISystem.instance.CreatePopup(LocalizationSystem.GetEntry("settings.valuechanged"), "menu.yes", "menu.no",
                () =>
                {
                    UISystem.instance.CloseCurrentWindow();
                    if (Apply())
                    {
                        UISystem.instance.CloseWindow(gameObject);
                    }
                    else
                    {
                        AskConfirmationForResolution();
                    }
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
                m_TMPPref.CurrentLanguage != pp.CurrentLanguage || m_TMPPref.Resolution != pp.Resolution || m_TMPPref.FrameLimit != pp.FrameLimit;
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

    public void OnDestroy()
    {
        UISystem.instance.onSelectionChangeEvent -= SetNewSelection;
    }
}

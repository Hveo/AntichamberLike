using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextReplacer : MonoBehaviour
{
    public LocalizedString LocalizedText;
    public TMP_Text TextComponent;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(LocalizedText.Key))
            return;

        LocalizationSystem.OnChangeLanguage += OnLanguageChange;
        LocalizedText.OnLanguageChange();
        SetText();
    }

    private void OnDisable()
    {
        LocalizationSystem.OnChangeLanguage -= OnLanguageChange;
    }

    private void OnDestroy()
    {
        LocalizationSystem.OnChangeLanguage -= OnLanguageChange;
    }

    void SetText()
    {
        if (TextComponent)
            TextComponent.text = LocalizedText.Content;
    }

    public void OnLanguageChange()
    {
        LocalizedText.OnLanguageChange();
        SetText();
    }
}

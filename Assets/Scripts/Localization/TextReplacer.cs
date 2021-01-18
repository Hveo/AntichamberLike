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
        LocalizationSystem.AddEntryInList(this);
        LocalizedText.OnLanguageChange();
        SetText();
    }

    private void OnDisable()
    {
        LocalizationSystem.RemoveEntryInList(this);
    }

    private void OnDestroy()
    {
        LocalizationSystem.RemoveEntryInList(this);
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

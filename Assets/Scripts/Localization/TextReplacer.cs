using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextReplacer : MonoBehaviour
{
    public LocalizedString LocalizedText;
    public TextMeshPro TextComponent;

    private void OnEnable()
    {
        LocalizationSystem.AddEntryInList(LocalizedText);
        LocalizedText.OnLanguageChange();
        SetText();
    }

    private void OnDisable()
    {
        LocalizationSystem.RemoveEntryInList(LocalizedText);
    }

    private void OnDestroy()
    {
        LocalizationSystem.RemoveEntryInList(LocalizedText);
    }

    void SetText()
    {
        if (TextComponent)
            TextComponent.text = LocalizedText.Content;
    }
}

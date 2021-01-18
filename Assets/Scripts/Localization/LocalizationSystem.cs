using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public enum Language
{
    FR = 0,
    EN = 1
}

public static class LocalizationSystem
{
    public static Language CurrentLanguage;
    private static Dictionary<string, string> LocalizedEntries;
    private static List<TextReplacer> DisplayedText; //CurrentObjects that are displayed and need to be updated real time when switching language

    public static void LoadLanguageEntries(Language lang)
    {
        if (lang == CurrentLanguage && LocalizedEntries != null)
            return;

        CurrentLanguage = lang;
        StreamReader read = new StreamReader(Application.streamingAssetsPath + "/Localization.xml");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(read.ReadToEnd());
        read.Close();

        XmlNodeList LocaKeys = xmlDoc.GetElementsByTagName(lang.ToString());
        LocalizedEntries = new Dictionary<string, string>();

        foreach (XmlNode node in LocaKeys)
        {
            LocalizedEntries.Add(node.ParentNode.Name, node.InnerText);
        }
    }

    public static void ChangeLanguage(Language lang)
    {
        LoadLanguageEntries(lang);

        for (int i = 0; i < DisplayedText.Count; ++i)
        {
            DisplayedText[i].OnLanguageChange();
        }
    }

    public static string GetEntry(string Key)
    {
        string result = "NULL";

        if (LocalizedEntries.ContainsKey(Key))
            result = LocalizedEntries[Key];

        return result;
    }

    public static void ClearDisplayedTextList()
    {
        if (DisplayedText != null)
            DisplayedText.Clear();
    }

    public static void AddEntryInList(TextReplacer textReplacer)
    {
        if (DisplayedText == null)
            DisplayedText = new List<TextReplacer>();

        if (!DisplayedText.Contains(textReplacer))
            DisplayedText.Add(textReplacer);
    }

    public static void RemoveEntryInList(TextReplacer textReplacer)
    {
        if (DisplayedText == null || !DisplayedText.Contains(textReplacer))
            return;

        DisplayedText.Remove(textReplacer);
    }
}

[Serializable]
public class LocalizedString
{
    public string Key;
    public string Content
    {
        get;
        private set;
    }

    public void OnLanguageChange()
    {
        Content = LocalizationSystem.GetEntry(Key);
    }
}
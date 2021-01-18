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
    public static Action OnChangeLanguage;
    private static Dictionary<string, string> LocalizedEntries;

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

        if (OnChangeLanguage != null && OnChangeLanguage.GetInvocationList() != null)
        {
            OnChangeLanguage.Invoke();
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
        OnChangeLanguage = null;
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
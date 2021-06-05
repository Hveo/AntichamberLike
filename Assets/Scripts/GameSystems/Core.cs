using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Core : MonoBehaviour
{
    public static Core instance;
    public BuiltInGameResources BuiltInResources;
    public PlayerPrefsObject PlayerPrefs;

    string m_PlayerPrefsPath;

    void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        LoadPlayerPrefs();
        LocalizationSystem.LoadLanguageEntries(PlayerPrefs.CurrentLanguage);
        StartCoroutine(AudioMgr.Init());
        InputHandler.InitiateInput();
    }

    void LoadPlayerPrefs()
    {
#if UNITY_STANDALONE_LINUX || UNITY_IOS
        m_PlayerPrefsPath = Application.persistentDataPath + "/Overthink";
#elif UNITY_STANDALONE_OSX
        m_PlayerPrefsPath = "~/Library/Application Support" + "/Overthink"
#else
        m_PlayerPrefsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/My Games/Overthink";

#endif
        if (!Directory.Exists(m_PlayerPrefsPath))
            Directory.CreateDirectory(m_PlayerPrefsPath);

        m_PlayerPrefsPath += "/PlayerPrefs.json";

        if (!File.Exists(m_PlayerPrefsPath))
        {
            string Json = JsonUtility.ToJson(PlayerPrefs);
            File.WriteAllText(m_PlayerPrefsPath, Json);
        }
        else
        {
            string Json = File.ReadAllText(m_PlayerPrefsPath);
            JsonUtility.FromJsonOverwrite(Json, PlayerPrefs);
        }
    }

    public void SavePlayerPrefs()
    {
        string Json = JsonUtility.ToJson(PlayerPrefs);
        File.WriteAllText(m_PlayerPrefsPath, Json);
    }
}

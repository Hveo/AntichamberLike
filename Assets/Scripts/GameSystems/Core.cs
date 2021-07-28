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
    public string DataPath { get; private set; }

    string m_PlayerPrefsPath;
    bool m_FileIsGenerated;

    void Awake()
    {
        m_FileIsGenerated = false;
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerPrefs();

            if (!m_FileIsGenerated)
            {
                int[] Res = PlayerPrefs.ParseResolution();
                Screen.SetResolution(Res[0], Res[1], true);
            }

            ApplyFrameLimit(PlayerPrefs.FrameLimit);

            LocalizationSystem.LoadLanguageEntries(PlayerPrefs.CurrentLanguage);
            StartCoroutine(AudioMgr.Init());
            InputHandler.InitiateInput();
        }
        else
            Destroy(gameObject);

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

        DataPath = m_PlayerPrefsPath;
        m_PlayerPrefsPath += "/PlayerPrefs.json";

        if (!File.Exists(m_PlayerPrefsPath))
        {
            string Json = JsonUtility.ToJson(PlayerPrefs);
            File.WriteAllText(m_PlayerPrefsPath, Json);
            m_FileIsGenerated = true;
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

    public void ApplyFrameLimit(string Limit)
    {
        if (string.CompareOrdinal(Limit, "30") == 0)
        {
            QualitySettings.vSyncCount = 2;
            Application.targetFrameRate = 30;
        }
        else if (string.CompareOrdinal(Limit, "60") == 0)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }
        else if (string.CompareOrdinal(Limit, "120") == 0)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }
        else if (string.CompareOrdinal(Limit, "menu.none") == 0)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
        }
    }
}

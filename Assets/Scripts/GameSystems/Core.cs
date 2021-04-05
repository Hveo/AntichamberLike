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
        string FilePath = Application.persistentDataPath + "/PlayerPrefs.json";

        if (!File.Exists(FilePath))
        {
            string Json = JsonUtility.ToJson(PlayerPrefs);
            File.WriteAllText(FilePath, Json);
        }
        else
        {
            string Json = File.ReadAllText(FilePath);
            JsonUtility.FromJsonOverwrite(Json, PlayerPrefs);
        }
    }
}

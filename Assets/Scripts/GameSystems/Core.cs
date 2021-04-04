using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        LocalizationSystem.LoadLanguageEntries(Language.EN);
        StartCoroutine(AudioMgr.Init());
        InputHandler.InitiateInput();
    }
}

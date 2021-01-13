using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public static Core instance;
    public BuiltInGameResources BuiltInResources;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        LocalizationSystem.LoadLanguageEntries(Language.EN);
        StartCoroutine(AudioMgr.Init());
    }
}

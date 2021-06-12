using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioMgr
{
    [System.Flags]
    public enum AudioType
    {
        SFX = (1 << 0),
        MUSIC = (1 << 1),
    };

    public static GameObject SoundHandler
    {
        get { return m_SoundHandler; }
        private set { }
    }
    
    private static GameObject m_SoundHandler;
    private static AudioSource[] m_SFXSources;
    private static AudioSource[] m_UISources;
    private static AudioSource m_MusicSource;
    private static AudioMixer m_Mixer;
    private static UISoundDB m_SoundDB;

    public static IEnumerator Init()
    {
        ResourceRequest request = Resources.LoadAsync("SoundHandlerSystem");

        while (!request.isDone)
            yield return null;

        if (request.asset != null)
        {
            m_SoundHandler = GameObject.Instantiate(request.asset) as GameObject;

            AudioSource[] sources = m_SoundHandler.GetComponents<AudioSource>();
            List<AudioSource> sfxs = new List<AudioSource>();
            List<AudioSource> uis = new List<AudioSource>();

            for (int i = 0; i < sources.Length; ++i)
            {
                if (string.CompareOrdinal(sources[i].outputAudioMixerGroup.name, "SFX") == 0)
                    sfxs.Add(sources[i]);
                else if (string.CompareOrdinal(sources[i].outputAudioMixerGroup.name, "UI") == 0)
                    uis.Add(sources[i]);
                else
                    m_MusicSource = sources[i];
            }

            m_SFXSources = sfxs.ToArray();
            m_UISources = uis.ToArray();
            GameObject.DontDestroyOnLoad(m_SoundHandler);
        }

        m_Mixer = Core.instance.BuiltInResources.Mixer;
        m_SoundDB = Core.instance.BuiltInResources.SoundDB;

        SetMusicVolume((int)Core.instance.PlayerPrefs.MusicVolume);
        SetSFXVolume((int)Core.instance.PlayerPrefs.FXVolume);
        SetUIVolume((int)Core.instance.PlayerPrefs.UIVolume);

    }

    public static void PlayUISound(string ID)
    {
        AudioClip clip = m_SoundDB.GetClip(ID);

        if (clip is null)
            return;

        for (int i = 0; i < m_UISources.Length; ++i)
        {
            if (m_UISources[i].isPlaying && clip != m_UISources[i].clip)
                continue;
            else
            {
                m_UISources[i].clip = clip;
                m_UISources[i].Play();
                return;
            }
        }

        m_UISources[0].clip = clip;
        m_UISources[0].Play();
    }

    public static void PlaySound(AudioClip clip, bool loop = false)
    {
        for (int i = 0; i < m_SFXSources.Length; ++i)
        {
            if (m_SFXSources[i].isPlaying)
                continue;
            else
            {
                m_SFXSources[i].clip = clip;
                m_SFXSources[i].loop = loop;
                m_SFXSources[i].Play();
            }
        }
    }

    public static void PlayMusic(AudioClip clip, bool loop = false, bool ensureVolume = false)
    {
        m_MusicSource.clip = clip;
        m_MusicSource.loop = loop;

        if (ensureVolume)
            m_MusicSource.volume = 1.0f;

        m_MusicSource.Play();
    }

    public static void StopMusic()
    {
        if (m_MusicSource != null) //Just to prevent Unplay error
            m_MusicSource.Stop();
    }

    public static void Pause(AudioType type)
    {
        if ((type & AudioType.SFX) != 0)
        {
            for (int i = 0; i < m_SFXSources.Length; ++i)
            {
                m_SFXSources[i].Pause();
            }
        }

        if ((type & AudioType.MUSIC) != 0)
            m_MusicSource.Pause();
    }

    public static void Resume(AudioType type)
    {
        if ((type & AudioType.SFX) != 0)
        {
            for (int i = 0; i < m_SFXSources.Length; ++i)
            {
                m_SFXSources[i].UnPause();
            }
        }

        if ((type & AudioType.MUSIC) != 0)
            m_MusicSource.UnPause();
    }

    public static void SetMusicVolume(int value)
    {
        SetFaderVolume("MusicVolume", value);
    }

    public static void SetSFXVolume(int value)
    {
        SetFaderVolume("SFXVolume", value);
    }

    public static void SetUIVolume(int value)
    {
        SetFaderVolume("UIVolume", value);
    }

    public static void SetLowpassValue(int value)
    {
        m_Mixer.SetFloat("MusicLowpass", value);
        m_Mixer.SetFloat("SFXLowpass", value);
    }

    static void SetFaderVolume(string Param, int value)
    {
        float normalizedValue = Mathf.Clamp((value - 80), -80, 5); //In order to keep ears alive
        m_Mixer.SetFloat(Param, normalizedValue);
    }

    public static IEnumerator FadeIn(float step, float delay = 0.0f)
    {
        if (step == 0.0f)
            yield break;
     
        m_MusicSource.volume = 0.0f;

        yield return new WaitForSeconds(delay);

        while (m_MusicSource.volume < 1.0f)
        {
            m_MusicSource.volume = Mathf.MoveTowards(m_MusicSource.volume, 1.0f, step);
            yield return null;
        }
    }
}

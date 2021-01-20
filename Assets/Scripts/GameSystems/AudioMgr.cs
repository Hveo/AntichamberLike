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
    private static AudioSource m_MusicSource;
    private static AudioMixer m_Mixer;

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

            for (int i = 0; i < sources.Length; ++i)
            {
                if (string.CompareOrdinal(sources[i].outputAudioMixerGroup.name, "SFX") == 0)
                    sfxs.Add(sources[i]);
                else
                    m_MusicSource = sources[i];
            }

            m_SFXSources = sfxs.ToArray();
            GameObject.DontDestroyOnLoad(m_SoundHandler);
        }

        m_Mixer = Core.instance.BuiltInResources.Mixer;
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

    public static void PlayMusic(AudioClip clip, bool loop = false)
    {
        m_MusicSource.clip = clip;
        m_MusicSource.loop = loop;
        m_MusicSource.Play();
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

    static void SetFaderVolume(string Param, int value)
    {
        float normalizedValue = Mathf.Clamp((value - 80), -80, 5); //In order to keep ears alive
        m_Mixer.SetFloat(Param, normalizedValue);
    }
}

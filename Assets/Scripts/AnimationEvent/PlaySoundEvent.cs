using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    public AudioClip SoundToPlay;
    public AudioSource AudioSrc;

    public void Start()
    {
        if (AudioSrc == null)
        {
            AudioSrc = GetComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        if (AudioSrc != null)
        {
            AudioSrc.clip = SoundToPlay;
            AudioSrc.Play();
        }
    }
}

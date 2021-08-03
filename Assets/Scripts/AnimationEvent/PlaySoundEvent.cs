using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    public AudioClip SoundToPlay;
    public AudioSource AudioSrc;
    public float Delay;

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
            if (Delay > 0.0f)
                StartCoroutine(DelayPlaySound());
            else
            {
                AudioSrc.clip = SoundToPlay;
                AudioSrc.Play();
            }
        }
        else
            AudioMgr.PlaySound(SoundToPlay);
    }

    IEnumerator DelayPlaySound()
    {
        yield return new WaitForSeconds(Delay);
        AudioSrc.clip = SoundToPlay;
        AudioSrc.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private AudioClip OpenSound;
    [SerializeField]
    private AudioClip UnlockSound;
    
    public Renderer DoorRenderer;
    public Material UnlockedMaterial;

    AudioSource m_AudioSource;
    Animator m_Animator;
    Material m_LockMaterial;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Animator = GetComponent<Animator>();

        if (DoorRenderer != null)
        {
            m_LockMaterial = DoorRenderer.materials[1];
        }
    }

    public void Open()
    {
        if (m_Animator != null)
            m_Animator.SetBool("Open", true);

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = OpenSound;
            m_AudioSource.Play();
        }
    }

    public void Close()
    {
        if (m_Animator != null)
            m_Animator.SetBool("Open", false);

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = OpenSound;
            m_AudioSource.Play();
        }
    }

    public void Unlock()
    {
        if (DoorRenderer == null)
            return;

        Material[] mat = DoorRenderer.materials;
        DoorRenderer.materials = new Material[] { mat[0], UnlockedMaterial };

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = UnlockSound;
            m_AudioSource.Play();
        }
    }

    public void Lock()
    {
        if (DoorRenderer == null)
            return;

        Material[] mat = DoorRenderer.materials;
        DoorRenderer.materials = new Material[] { mat[0], m_LockMaterial };

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = UnlockSound;
            m_AudioSource.Play();
        }
    }
}

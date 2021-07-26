using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : IInteractible
{
    public UnityEvent OnPush;
    public bool TriggerOnce;
    public Renderer ButtonRenderer;
    public Renderer BaseRenderer;
    public Color HighlightButtonColor;
    public Color HighlightBaseColor;
    public AudioClip ButtonClickSound;

    bool m_InteractDelayOver;
    Color m_OriginalButtonEmission;
    Color m_OriginalBaseEmission;
    Animator m_Animator;
    AudioSource m_AudioSource;

    public void Start()
    {
        IsInteractible = true;
        m_InteractDelayOver = true;
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_OriginalButtonEmission = ButtonRenderer.material.GetColor("_EmissionColor");
        m_OriginalBaseEmission = BaseRenderer.material.GetColor("_EmissionColor");
    }

    public override void Interact()
    {
        if (!m_InteractDelayOver)
            return;

        if (m_Animator != null)
            m_Animator.SetTrigger("Push");

        if (m_AudioSource != null)
        {
            m_AudioSource.clip = ButtonClickSound;
            m_AudioSource.Play();
        }

        if (TriggerOnce)
        {
            SetInteractibilityState(false);
            OnStopBeingInteractible();
        }

        if (OnPush != null)
            OnPush.Invoke();

        m_InteractDelayOver = false;
        StartCoroutine(InteractDelay());
    }

    IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(0.5f);
        m_InteractDelayOver = true;

    }

    public override void OnBeingInteractible()
    {
        ButtonRenderer.material.SetColor("_EmissionColor", HighlightButtonColor);
        BaseRenderer.material.SetColor("_EmissionColor", HighlightBaseColor);
    }

    public override void OnStopBeingInteractible()
    {
        ButtonRenderer.material.SetColor("_EmissionColor", m_OriginalButtonEmission);
        BaseRenderer.material.SetColor("_EmissionColor", m_OriginalBaseEmission);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelookBehaviour : MonoBehaviour
{
    EventListener m_Listener;
    Transform m_PlayerCamTransform;
    Material m_EyeMat;
    float m_Timer;
    AudioSource m_audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        if (m_Listener == null)
            m_Listener = GetComponent<EventListener>();
        
        if (m_EyeMat == null)
            m_EyeMat = GetComponent<Renderer>().material;

        if (m_audioSrc == null)
            m_audioSrc = GetComponent<AudioSource>();

        m_PlayerCamTransform = Camera.main.transform;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Timer <= 0.0f)
            return;

        if (Vector3.Dot(m_PlayerCamTransform.forward, transform.up) < -0.97)
        {
            m_Timer -= Time.deltaTime;

            if (m_Timer <= 0.0f && m_EyeMat != null)
                StartCoroutine(EyeActivate());
        }
        else
            Reset();
                    
    }

    public void Reset()
    {
        m_Timer = 1.0f;
    }

    IEnumerator EyeActivate()
    {
        m_audioSrc.Play();
        yield return StartCoroutine(CloseEye());
        yield return StartCoroutine(OpenEye());

        if (m_Listener != null)
            m_Listener.ToggleEffectBypass();

        Reset();
    }

    IEnumerator EyeDisable()
    {
        if (m_EyeMat != null)
            yield return StartCoroutine(OpenEye());
        
        Reset();
    }

    IEnumerator CloseEye()
    {
        float BlinkValue = m_EyeMat.GetFloat("_Blink");

        while (BlinkValue < 1.0f)
        {
            BlinkValue = Mathf.MoveTowards(BlinkValue, 1.0f, Time.deltaTime * 1.75f);
            m_EyeMat.SetFloat("_Blink", BlinkValue);
            yield return null;
        }
    }

    IEnumerator OpenEye()
    {
        float BlinkValue = m_EyeMat.GetFloat("_Blink");

        while (BlinkValue > 0.0f)
        {
            BlinkValue = Mathf.MoveTowards(BlinkValue, 0.0f, Time.deltaTime * 1.75f);
            m_EyeMat.SetFloat("_Blink", BlinkValue);
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        LevelMgr.instance.StartCoroutine(OpenEye());
        Reset();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelookBehaviour : MonoBehaviour
{
    EventListener m_Listener;
    Transform m_PlayerCamTransform;
    Material m_EyeMat;
    float m_Timer;

    // Start is called before the first frame update
    void Start()
    {
        m_Listener = GetComponent<EventListener>();
        m_PlayerCamTransform = Camera.main.transform;
        m_EyeMat = GetComponent<Renderer>().material;
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
            StartCoroutine(EyeActivate());
        }
        else
            Reset();
                    
    }

    public void Reset()
    {
        m_Timer = 2.0f;
    }

    IEnumerator EyeActivate()
    {
        if (m_EyeMat == null)
            yield break;

        float BlinkValue = m_EyeMat.GetFloat("_Blink"); 
        
        while (BlinkValue > 0.0f)
        {
            BlinkValue = Mathf.MoveTowards(BlinkValue, 0.0f, Time.deltaTime * 1.75f);
            m_EyeMat.SetFloat("_Blink", BlinkValue);
            yield return null;
        }

        while (BlinkValue < 1.0f)
        {
            BlinkValue = Mathf.MoveTowards(BlinkValue, 1.0f, Time.deltaTime * 1.75f);
            m_EyeMat.SetFloat("_Blink", BlinkValue);
            yield return null;
        }

        if (m_Listener != null)
            m_Listener.ToggleEffectBypass();

        Reset();
    }
}

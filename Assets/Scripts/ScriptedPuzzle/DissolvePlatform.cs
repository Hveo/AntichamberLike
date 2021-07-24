using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePlatform : MonoBehaviour
{
    public AnimatorSetValue AnimatorValueModifier;
    public BoxCollider ToDisable;

    Material m_Mat;
    float m_Timer;
    float m_Amount;
    bool m_Toggled;
    bool m_SoundPlayed;
    bool m_CanDestroy;

    private void Start()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        m_Timer = 2.0f;
        m_Toggled = false;
        m_CanDestroy = false;

        if (rend != null)
        {
            m_Mat = rend.material;
            m_Mat.SetFloat("_Amount", 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AnimatorValueModifier.intParam = 1;
        AnimatorValueModifier.SetValue();
    }

    private void OnTriggerStay(Collider other)
    {
        m_Timer -= Time.deltaTime;

        if (!m_Toggled && m_Timer <= 0.0f)
        {
            m_Amount = 0.0f;
            m_Toggled = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_CanDestroy)
            return;

        if (m_Toggled)
        {
            if (!m_SoundPlayed)
            {
                m_SoundPlayed = true;
                GetComponent<AudioSource>().Play();
            }

            m_Amount = Mathf.MoveTowards(m_Amount, 1.0f, Time.deltaTime * 0.5f);
            m_Mat.SetFloat("_Amount", m_Amount);

            if (m_Amount >= 1.0f)
            {
                Destroy(AnimatorValueModifier.gameObject);
                ToDisable.enabled = false;
                m_CanDestroy = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (m_CanDestroy)
        {
            Destroy(gameObject);
        }

        m_Timer = 2.0f;

        if (AnimatorValueModifier != null)
        {
            AnimatorValueModifier.intParam = 2;
            AnimatorValueModifier.SetValue();
        }
    }
}

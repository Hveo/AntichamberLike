using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePlatform : MonoBehaviour
{
    public AnimatorSetValue AnimatorValueModifier;

    Material m_Mat;
    float m_Timer;
    float m_Amount;
    bool m_Toggled;

    private void Start()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        m_Timer = 2.0f;
        m_Toggled = false;

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
        if (m_Toggled)
        {
            m_Amount = Mathf.MoveTowards(m_Amount, 1.0f, Time.deltaTime * 0.75f);
            m_Mat.SetFloat("_Amount", m_Amount);

            if (m_Amount >= 1.0f)
            {
                Destroy(AnimatorValueModifier.gameObject);
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        m_Timer = 2.0f;

        AnimatorValueModifier.intParam = 2;
        AnimatorValueModifier.SetValue();
    }
}

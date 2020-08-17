using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothColorChange : MonoBehaviour
{
    Material m_ParticleMat;
    public Color Color;
    public bool AlterEmissive;

    private void Start()
    {
        m_ParticleMat = GetComponent<ParticleSystemRenderer>().material;
    }

    public void ChangeColor(float duration)
    {
        if (m_ParticleMat != null)
            StartCoroutine(MoveTowardColor(Color, duration));
    }

    IEnumerator MoveTowardColor(Color col, float duration)
    {
        Color crtColor = m_ParticleMat.GetColor("_Color");
        Color emissive = m_ParticleMat.GetColor("_EmissionColor");

        while (duration > 0.0f)
        {
            crtColor = Color.Lerp(crtColor, col, (duration - 0.5f) * Time.deltaTime);
            m_ParticleMat.SetColor("_Color", crtColor);

            if (AlterEmissive)
                m_ParticleMat.SetColor("_EmissionColor", crtColor);

            yield return null;
            duration -= Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    Material[] m_Mat;

    // Start is called before the first frame update
    void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        m_Mat = new Material[rends.Length];

        for (int i = 0; i < rends.Length; ++i)
        {
            m_Mat[i] = rends[i].material;
            m_Mat[i].SetFloat("_Amount", 0.0f);
            m_Mat[i].SetColor("_Color", Color.magenta);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        float Amount = 0.0f;
        while (Amount < 1.0f)
        {
            Amount = Mathf.MoveTowards(Amount, 1.0f, Time.deltaTime * 0.75f);

            for (int i = 0; i < m_Mat.Length; ++i)
            {
                m_Mat[i].SetFloat("_Amount", Amount);
            }

            yield return null;
        }

        //Move To Secret Lock Chamber And Set State

        for (int i = 0; i < m_Mat.Length; ++i)
        {
            m_Mat[i].SetFloat("_Amount", 0.0f);
        }
    }
}

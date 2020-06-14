using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DistanceDissolve : MonoBehaviour
{
    public bool DissolveOnApproach;
    public float DistBeforeEffect;

    List<Material> m_MatInstances;
    Transform m_PlayerTransform;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_PlayerTransform = GameMgr.instance.Player.transform;
        Renderer[] Rends = GetComponentsInChildren<Renderer>();
        m_MatInstances = new List<Material>(); 

        for (int i = 0; i < Rends.Length; ++i)
        {
            if (Rends[i].material != null && Rends[i].material.HasProperty("_Amount"))
            {
                m_MatInstances.Add(Rends[i].material);

                if (!DissolveOnApproach)
                    Rends[i].material.SetFloat("_Amount", 1.0f);
            }
        }


        while (true)
        {
            float dist = Vector3.Distance(m_PlayerTransform.position, transform.position);

            if (dist < DistBeforeEffect)
            {
                float value = (dist - 0.5f) / (DistBeforeEffect - 0.5f);

                if (DissolveOnApproach)
                    value = 1.0f - value;

                for (int i = 0; i < m_MatInstances.Count; ++i)
                {
                    m_MatInstances[i].SetFloat("_Amount", value);
                }
            } 
            
            yield return null;
        }
    }
}

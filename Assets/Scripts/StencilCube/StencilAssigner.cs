using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StenciledRenderers
{
    public Renderer[] rends;
    public Color Color;
}

public class StencilAssigner : MonoBehaviour
{
    public Renderer[] Faces;
    public List<StenciledRenderers> Objects;

    void Start()
    {
        if (Faces == null || Objects == null)
            return;

        for (int i = 0; i < Faces.Length; ++i)
        {
            if (Faces[i] == null)
                continue;

            Faces[i].material.SetFloat("_StencilReferenceID", i);
        }

        for (int i = 0; i < Objects.Count; ++i)
        {
            if (Objects[i] == null)
                continue;

            for (int j = 0; j < Objects[i].rends.Length; ++j)
            {
                if (Objects[i].rends[j] == null)
                    continue;

                Objects[i].rends[j].material.SetColor("_Color", Objects[i].Color);
                Objects[i].rends[j].material.SetFloat("_StencilReferenceID", i);
            }
        }
    }
}


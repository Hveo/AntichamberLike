using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSequencer : MonoBehaviour
{
    public int ScaleOccurences;
    // Start is called before the first frame update
    bool stopSeq;

    private void Start()
    {
        var seq = LeanTween.sequence();

        for (int i = 0; i < ScaleOccurences; ++i)
        {
            seq.append(LeanTween.scale(transform.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.2f));
            seq.append(LeanTween.scale(transform.gameObject, Vector3.one, 0.2f));
        }

        seq.append(2.0f);

        if (!stopSeq)
            seq.append(Start);
        else
            Destroy(this);
    }

    public void StopSequence()
    {
        stopSeq = true;
    }
}

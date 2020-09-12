using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rends.Length; ++i)
        {
            rends[i].material.SetFloat("_Amount", 0.0f);
            rends[i].material.SetColor("_Color", Color.magenta);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        GetComponent<Animator>().enabled = false;
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rends.Length; ++i)
        {
            StartCoroutine(GameUtilities.DissolveMesh(rends[i], true, 0.75f));
        }

        while (rends[rends.Length - 1].material.GetFloat("_Amount") < 1.0f)
            yield return null;

        //Move To Secret Lock Chamber And Set State
        GameMgr.instance.IncrementStateValue("KeyCollected");

        for (int i = 0; i < rends.Length; ++i)
        {
            rends[i].material.SetFloat("_Amount", 0.0f);
        }
    }
}

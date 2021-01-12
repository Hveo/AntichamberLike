using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip ClipToPlayOnCollect;
    public string StateToIncrement;

    // Start is called before the first frame update
    void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rends.Length; ++i)
        {
            rends[i].material.SetFloat("_Amount", 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        GetComponent<Animator>().enabled = false;

        if (ClipToPlayOnCollect != null)
            AudioMgr.PlaySound(ClipToPlayOnCollect);
        
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rends.Length; ++i)
        {
            StartCoroutine(GameUtilities.DissolveMesh(rends[i], true, 0.75f));
        }

        while (rends[rends.Length - 1].material.GetFloat("_Amount") < 1.0f)
            yield return null;

        GameMgr.instance.IncrementStateValue(StateToIncrement);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendingCorridor : MonoBehaviour
{
    public Gradient Gradient;
    public Renderer[] Renderers;
    private void OnTriggerStay(Collider other)
    {
        float forwardVelocity = Vector3.Dot(LevelMgr.instance.Player.PlayerBody.velocity, LevelMgr.instance.Player.transform.forward);
        float relativeOrientation = Vector3.Dot(transform.parent.forward, LevelMgr.instance.Player.transform.forward); 
        //Vector3.Dot(GameMgr.instance.Player.PlayerBody.velocity.z)
        float newScale = Mathf.Clamp((transform.parent.localScale.z + (relativeOrientation * forwardVelocity * Time.deltaTime) * 0.07f), 1.0f, 8.0f);
        transform.parent.localScale = new Vector3(1, 1, newScale);
        AdaptCorridorColorToCurrentLength(newScale);
    }

    void AdaptCorridorColorToCurrentLength(float currentScale)
    {
        float normalized = (currentScale - 1.0f) / 7.0f;
        Color col = Gradient.Evaluate(normalized);

        for (int i = 0; i < Renderers.Length; ++i)
        {
            Material mat = Renderers[i].material;
            mat.SetColor("_EmissionColor", col);
            mat.SetColor("_Color", col);
        }
    }
}

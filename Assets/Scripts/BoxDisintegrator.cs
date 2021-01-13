using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDisintegrator : MonoBehaviour
{
    public Transform RespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < other.transform.childCount; ++i)
            {
                PhysicBox box = other.transform.GetChild(i).GetComponent<PhysicBox>();
                
                if (box != null)
                    box.Interact();
            }
        }
        else
        {
            StartCoroutine(DestroyAndRespawn(other.gameObject));
        }
    }

    IEnumerator DestroyAndRespawn(GameObject obj)
    {
        yield return GameUtilities.DissolveMesh(obj.GetComponent<Renderer>(), true, 0.75f);
        obj.transform.position = RespawnPoint.position;
        yield return GameUtilities.DissolveMesh(obj.GetComponent<Renderer>(), false, 0.75f);
    }

    public void DestroyAndRespawn_Utility(GameObject obj)
    {
        StartCoroutine(DestroyAndRespawn(obj));
    }
}

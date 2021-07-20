using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDisintegrator : MonoBehaviour
{
    public Transform RespawnPoint;
    AudioSource m_Src;
    List<GameObject> IgnoreList;

    public void Start()
    {
        m_Src = GetComponent<AudioSource>();
        IgnoreList = new List<GameObject>();
    }

    private void OnTriggerStay(Collider other)
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
            if (!IgnoreList.Contains(other.gameObject))
            {
                IgnoreList.Add(other.gameObject);
                StartCoroutine(DestroyAndRespawn(other.gameObject));
            }
        }
    }

    IEnumerator DestroyAndRespawn(GameObject obj)
    {
        m_Src.Play();
        yield return GameUtilities.DissolveMesh(obj.GetComponent<Renderer>(), true, 0.75f);
        obj.transform.position = RespawnPoint.position;
        yield return GameUtilities.DissolveMesh(obj.GetComponent<Renderer>(), false, 0.75f);
        IgnoreList.Remove(obj);
    }

    public void DestroyAndRespawn_Utility(GameObject obj)
    {
        StartCoroutine(DestroyAndRespawn(obj));
    }
}

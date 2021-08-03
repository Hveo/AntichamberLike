using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecretRoom : MonoBehaviour
{
    public GameObject[] Keys;
    public Renderer[] Locks;

    bool[] keyInserted;
    Animator m_Animator;
    Renderer[] m_KeyRenderers;
    short m_DoorState;

    private void Start()
    {
        m_DoorState = 0;
        keyInserted = new bool[] { false, false };
        m_Animator = GetComponent<Animator>();
        m_KeyRenderers = new Renderer[4];

        for (int i = 0; i < Keys.Length; ++i)
        {
            int offset = i;
            for (int j = 0; j < Keys[i].transform.childCount; ++j)
            {
                m_KeyRenderers[i + j + offset] = Keys[i].transform.GetChild(j).GetComponent<Renderer>();
                m_KeyRenderers[i + j + offset].material.SetFloat("_Amount", 1.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (keyInserted[0] && keyInserted[1])
            return;

        if (LevelMgr.instance.GetStateValue("GreenKey") == 1 && !keyInserted[0])
        {
            StartCoroutine(GameUtilities.DissolveMesh(m_KeyRenderers[0], false, 0.5f));
            StartCoroutine(GameUtilities.DissolveMesh(m_KeyRenderers[1], false, 0.5f));
            Keys[0].GetComponent<Animator>().SetTrigger("InsertKey");
            keyInserted[0] = true;
            StartCoroutine(NotifyKeyInserted(false));
        }
        
        if (LevelMgr.instance.GetStateValue("RedKey") == 1 && !keyInserted[1])
        {
            StartCoroutine(GameUtilities.DissolveMesh(m_KeyRenderers[2], false, 0.5f));
            StartCoroutine(GameUtilities.DissolveMesh(m_KeyRenderers[3], false, 0.5f));
            Keys[1].GetComponent<Animator>().SetTrigger("InsertKey");
            keyInserted[1] = true;
            StartCoroutine(NotifyKeyInserted(true));
        }
    }

    IEnumerator NotifyKeyInserted(bool right)
    {
        yield return new WaitForSeconds(2.0f);

        int index = right ? 1 : 0;

        Locks[index].material.SetColor("_Color", Color.green);
        Locks[index].material.SetColor("_EmissionColor", Color.green);
        m_DoorState++;

        if (m_DoorState == 2)
        {
            m_Animator.SetTrigger("OpenDoor");
            
            if (Core.instance.BuiltInResources)
                AudioMgr.PlaySound(Core.instance.BuiltInResources.SolvedPuzzleClip);
        }
    }
}

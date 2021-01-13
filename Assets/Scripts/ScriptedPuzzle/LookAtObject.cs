using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookAtObject : MonoBehaviour
{
    public GameObject Object;
    public bool ShouldNotLookAt;
    public bool DisableOnSuccess;
    public float LookThreshold;
    public string StateID;

    private Transform m_PlayerTransform;
    private bool m_CheckCondition;

    private void Start()
    {
        LevelMgr.instance.SetStateValue(StateID, 0);
        m_PlayerTransform = LevelMgr.instance.Player.transform;
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        float LookPercentage = Vector3.Dot(Camera.main.transform.forward, (Object.transform.position - m_PlayerTransform.position).normalized);

        if (LookPercentage > LookThreshold)
        {
            if (ShouldNotLookAt)
                LevelMgr.instance.SetStateValue(StateID, 0);
            else
            {
                LevelMgr.instance.SetStateValue(StateID, 1);

                if (DisableOnSuccess)
                    Destroy(this);
            }
        }
        else
        {
            if (ShouldNotLookAt)
            {
                LevelMgr.instance.SetStateValue(StateID, 1);

                if (DisableOnSuccess)
                    Destroy(this);
            }
            else
                LevelMgr.instance.SetStateValue(StateID, 0);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookAtObject : MonoBehaviour
{
    public GameObject Object;
    public bool ShouldNotLookAt;
    public float LookThreshold;
    public string StateID;

    private Transform m_PlayerTransform;

    private void Start()
    {
        GameMgr.instance.SetStateValue(StateID, 0);
        m_PlayerTransform = GameMgr.instance.Player.transform;
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
            GameMgr.instance.SetStateValue(StateID, ShouldNotLookAt ? 0 : 1);
        else
            GameMgr.instance.SetStateValue(StateID, ShouldNotLookAt ? 1 : 0);

    }
}

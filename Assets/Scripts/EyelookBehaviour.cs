using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelookBehaviour : MonoBehaviour
{
    EventListener m_Listener;
    Transform m_PlayerCamTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_Listener = GetComponent<EventListener>();
        m_PlayerCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(m_PlayerCamTransform.forward, transform.up) < -0.98)
            Debug.Log("You Look at me");
    }
}

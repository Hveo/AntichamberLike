using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public Rigidbody[] Boxes;
    public bool ChangeGravity;
    public EyelookBehaviour Activator;

    int m_GravityState = 0;
    bool m_PlayerIn;
    // Update is called once per frame
    void Update()
    {
        if (ChangeGravity)
        {
            ChangeGravity = false;
            m_GravityState = m_GravityState == 0 ? 1 : 0;

            if (m_GravityState == 1)
                Physics.gravity = new Vector3(0.0f, 9.81f, 0.0f);
            else
                Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_PlayerIn = true;
        for (int i = 0; i < Boxes.Length; ++i)
            Boxes[i].useGravity = true;

        Activator.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        m_PlayerIn = false;
        Activator.enabled = false;
    }
}

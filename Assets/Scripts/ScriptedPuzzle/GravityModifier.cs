using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public PhysicBox[] Boxes;
    public EyelookBehaviour Activator;

    int m_GravityState = 0;
    // Update is called once per frame

    private void FixedUpdate()
    {
        Vector3 gravity = m_GravityState == 0 ? -Vector3.up * Physics.gravity.magnitude : Vector3.up * Physics.gravity.magnitude;

        for (int i = 0; i < Boxes.Length; ++i)
        {
            if (Boxes[i].IsCarried)
            {
                GameMgr.instance.Player.Controller.Move(gravity * Time.deltaTime);
            }
            else
                Boxes[i].Body.AddForce(gravity);
        }
    }

    public void ChangeGravity()
    {
        m_GravityState = m_GravityState == 0 ? 1 : 0;
        for (int i = 0; i < Boxes.Length; ++i)
            Boxes[i].Body.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < Boxes.Length; ++i)
            Boxes[i].Body.useGravity = true;

        Activator.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        GameMgr.instance.Player.ToggleJumpAvailability(m_GravityState == 0);
    }

    private void OnTriggerExit(Collider other)
    {
        Activator.enabled = false;
        GameMgr.instance.Player.ToggleJumpAvailability(true);
    }
}

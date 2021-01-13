using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            Boxes[i].Body.AddForce(gravity);

            if (Boxes[i].IsCarried)
                LevelMgr.instance.Player.PlayerBody.AddForce(gravity * 10.0f, ForceMode.Force);
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
        this.enabled = true;
        for (int i = 0; i < Boxes.Length; ++i)
            Boxes[i].Body.useGravity = true;

        Activator.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        LevelMgr.instance.Player.ToggleJumpAvailability(m_GravityState == 0);
    }

    private void OnTriggerExit(Collider other)
    {
        //this.enabled = false;
        Activator.enabled = false;
        LevelMgr.instance.Player.ToggleJumpAvailability(true);
    }
}

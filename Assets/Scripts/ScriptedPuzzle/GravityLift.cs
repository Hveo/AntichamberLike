using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLift : MonoBehaviour
{
    bool PlayerIn;

    private void OnTriggerEnter(Collider other)
    {
        PlayerIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerIn = false;
    }

    private void FixedUpdate()
    {
        if (PlayerIn)
        {
            Vector3 gravity = Vector3.up * Physics.gravity.magnitude;
            GameMgr.instance.Player.PlayerBody.AddForce(gravity * 12.0f, ForceMode.Force);
        }
    }
}

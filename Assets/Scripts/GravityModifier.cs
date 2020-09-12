using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public bool ChangeGravity;
    int GravityState = 0;

    // Update is called once per frame
    void Update()
    {
        if (ChangeGravity)
        {
            ChangeGravity = false;
            GravityState = GravityState == 0 ? 1 : 0;

            if (GravityState == 1)
                Physics.gravity = new Vector3(0.0f, 9.81f, 0.0f);
            else
                Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);

        }
    }
}

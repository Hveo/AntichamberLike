using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    
    public Behaviour[] ComponentsToCull;
    public int DistanceBeforeCulling;

    // Update is called once per frame
    void LateUpdate()
    {
        if (ComponentsToCull == null)
            return;

        if (Vector3.Distance(LevelMgr.instance.Player.transform.position, transform.position) >= DistanceBeforeCulling)
        {
            for (int i = 0; i < ComponentsToCull.Length; ++i)
                ComponentsToCull[i].enabled = false;
        }
        else
        {
            for (int i = 0; i < ComponentsToCull.Length; ++i)
                ComponentsToCull[i].enabled = true;
        }
    }
}

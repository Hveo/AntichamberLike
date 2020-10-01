using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandelRoomStairs : MonoBehaviour
{
    private AnimatorSetValue animatorValueChanger;

    private void Start()
    {
        animatorValueChanger = GetComponent<AnimatorSetValue>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animatorValueChanger)
        {
            animatorValueChanger.boolParam = true;
            animatorValueChanger.SetValue();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (animatorValueChanger)
        {
            animatorValueChanger.boolParam = false;
            animatorValueChanger.SetValue();
        }
    }
}

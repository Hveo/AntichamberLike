using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecretRoom : MonoBehaviour
{
    public AnimatorSetValue AnimatorModifier;

    private void OnTriggerEnter(Collider other)
    {
        if (GameMgr.instance.GetStateValue("KeyCollected") >= 2)
        {
            AnimatorModifier.SetValue();
            Destroy(gameObject);
        }
    }
}

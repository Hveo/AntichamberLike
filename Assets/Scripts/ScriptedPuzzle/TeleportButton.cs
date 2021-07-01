using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public void TeleportPlayer()
    {
        GameUtilities.TeleportPlayerTo(new Vector3(-2.00908f, 1.52f, -6.891097f), Vector3.forward);
    }
}

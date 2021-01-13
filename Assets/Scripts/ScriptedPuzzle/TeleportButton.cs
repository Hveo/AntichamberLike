using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public void TeleportPlayer()
    {
        GameUtilities.TeleportPlayerTo(new Vector3(3.33f, 1.5f, -3.46f), LevelMgr.instance.Player.transform.forward);
    }
}

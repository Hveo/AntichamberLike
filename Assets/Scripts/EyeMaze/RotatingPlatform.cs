using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        SetNewRotation(LevelMgr.instance.GetStateValue("MazeWallState") == 1);
    }

    public void SetNewRotation(bool up)
    {
        if (up)
            transform.LeanRotateY(180.0f, 1.5f);
        else
            transform.LeanRotateY(0.0f, 1.5f);
    }
}

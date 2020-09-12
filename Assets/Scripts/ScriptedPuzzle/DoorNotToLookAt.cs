using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNotToLookAt : MonoBehaviour
{
    public Door DoorNotToLook;
    public float LookThreshold;
    private Transform PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameMgr.instance.SetStateValue("LookAtDoor", 0);
        PlayerTransform = GameMgr.instance.Player.transform;
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
    }

    private void Update()
    {

        float LookPercentage = Vector3.Dot(Camera.main.transform.forward, (DoorNotToLook.transform.position - PlayerTransform.position).normalized);

        if (LookPercentage > LookThreshold)
            GameMgr.instance.SetStateValue("LookAtDoor", 0);
        else
            GameMgr.instance.SetStateValue("LookAtDoor", 1);
    }
}

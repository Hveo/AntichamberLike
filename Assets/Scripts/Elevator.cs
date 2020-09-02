using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform[] AnchorPoints;
    public EyelookBehaviour[] EyeToActivate;

    private Vector3 m_Velocity;
    private int m_IndexDestination;
    private bool m_PlayerOverride;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EyeToActivate != null)
        {
            for (int i = 0; i < EyeToActivate.Length; ++i)
                EyeToActivate[i].enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (EyeToActivate != null)
        {
            for (int i = 0; i < EyeToActivate.Length; ++i)
                EyeToActivate[i].enabled = false;
        }
    }
}

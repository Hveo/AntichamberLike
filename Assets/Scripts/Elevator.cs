using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform[] AnchorPoints;
    public EyelookBehaviour[] EyeToActivate;
    public float MaxSpeed;

    private float Acceleration;
    private Vector3 m_Destination;
    private bool m_PlayerIn;
    private AudioSource m_Audio;

    // Start is called before the first frame update
    void Start()
    {
        m_Destination = AnchorPoints[1].position;
        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Direction = m_Destination - transform.position;
        float magnitude =  Direction.magnitude;
        
        if (magnitude > 0.1f)
        {
            if (m_PlayerIn)
                GameMgr.instance.Player.transform.parent = transform;
            else
                GameMgr.instance.Player.transform.parent = null;

            DeactivateEye();

            Acceleration += Time.deltaTime * MaxSpeed;
            Acceleration = Mathf.Min(Acceleration, magnitude);

            transform.position += (Direction.normalized) * Acceleration * Time.deltaTime;

            if (m_Audio)
                m_Audio.pitch = Mathf.Clamp(Acceleration, 0.0f, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EyeToActivate != null)
        {
            m_PlayerIn = true;
            GameMgr.instance.Player.ToggleJumpAvailability(false);
            
            for (int i = 0; i < EyeToActivate.Length; ++i)
                EyeToActivate[i].enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_PlayerIn = false;
        GameMgr.instance.Player.ToggleJumpAvailability(true);

        DeactivateEye();
    }

    void DeactivateEye()
    {
        if (EyeToActivate != null)
        {
            for (int i = 0; i < EyeToActivate.Length; ++i)
                EyeToActivate[i].enabled = false;
        }
    }

    public void MoveTo(int AnchorIndex)
    {
        if (AnchorIndex > -1 && AnchorIndex < AnchorPoints.Length)
        {
            m_Destination = AnchorPoints[AnchorIndex].position;
        }
    }
}

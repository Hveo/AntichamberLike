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
        m_Destination = AnchorPoints[0].position;
        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_PlayerIn)
        {
            LevelMgr.instance.Player.transform.parent = null;
            MoveTo(GetClosestAnchorPoint());
        }

        Vector3 Direction = m_Destination - transform.position;
        float magnitude =  Direction.magnitude;

        if (magnitude > 0.1f)
        {
            if (m_PlayerIn)
                LevelMgr.instance.Player.transform.parent = transform;
            
            DisableEyes();

            Acceleration += Time.deltaTime * MaxSpeed;
            Acceleration = Mathf.Min(Acceleration, magnitude);

            transform.position += (Direction.normalized) * Acceleration * Time.deltaTime;

            if (m_Audio)
                m_Audio.pitch = Mathf.Clamp(Acceleration, 0.0f, 1.0f);
        }
        else if (m_PlayerIn)
            EnableEyes();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnableEyes();
    }

    private void OnTriggerExit(Collider other)
    {
        m_PlayerIn = false;
        LevelMgr.instance.Player.ToggleJumpAvailability(true);

        DisableEyes();
    }

    void EnableEyes()
    {
        if (EyeToActivate != null)
        {
            m_PlayerIn = true;
            LevelMgr.instance.Player.ToggleJumpAvailability(false);

            for (int i = 0; i < EyeToActivate.Length; ++i)
                EyeToActivate[i].enabled = true;
        }
    }

    void DisableEyes()
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

    int GetClosestAnchorPoint()
    {
        Vector3 playerPos = LevelMgr.instance.Player.transform.position;
        float bestDistance = float.MaxValue;
        int AnchorPointIndex = 0;

        for (int i = 0; i < AnchorPoints.Length; ++i)
        {
            if (bestDistance > Vector3.Distance(AnchorPoints[i].position, playerPos))
            {
                bestDistance = Vector3.Distance(AnchorPoints[i].position, playerPos);
                AnchorPointIndex = i;
            }
        }

        return AnchorPointIndex;
    }
}

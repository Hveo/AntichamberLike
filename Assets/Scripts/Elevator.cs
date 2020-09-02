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
    private bool m_PlayerOverride;
    private AudioSource m_Audio;

    // Start is called before the first frame update
    void Start()
    {
        m_Destination = AnchorPoints[1].position;
        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Destination != transform.position)
        {
            DeactivateEye();

            Acceleration += Time.deltaTime * 1.0f;
            Acceleration = Mathf.Min(Acceleration, MaxSpeed);
            transform.position = Vector3.Lerp(transform.position, m_Destination, Acceleration);

            if (m_Audio)
                m_Audio.pitch = Mathf.Clamp(Acceleration, 0.0f, 1.5f);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody PlayerBody
    { 
        get
        {
            if (m_PlayerBody == null)
                m_PlayerBody = GetComponent<Rigidbody>();

            return m_PlayerBody;
        }

        private set { }
    }

    public Vector3 Center
    {
        get
        {
            return GetComponent<CapsuleCollider>().center;
        }

        private set { }
    }

    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_MaxVelocityChange;
    private float m_JumpMovementFactor;

    private AudioSource m_AudioSrc;
    private Rigidbody m_PlayerBody;
    private float m_InitialJumpForce;

    [SerializeField] private float jumpMultiplier;

    private void Awake()
    {
        m_InitialJumpForce = jumpMultiplier;
        m_AudioSrc = GetComponent<AudioSource>();
    }



    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= m_MovementSpeed * m_JumpMovementFactor;

        Vector3 velocity = m_PlayerBody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -m_MaxVelocityChange, m_MaxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -m_MaxVelocityChange, m_MaxVelocityChange);
        velocityChange.y = 0;
        m_PlayerBody.AddForce(velocityChange, ForceMode.VelocityChange);

        JumpInput(velocity);
    }

    private void JumpInput(Vector3 velocity)
    {
        if (!IsGrounded())
        {
            m_JumpMovementFactor = Mathf.Lerp(m_JumpMovementFactor, 0.3f, Time.deltaTime);
            m_PlayerBody.AddForce(new Vector3(0, Physics.gravity.y * m_PlayerBody.mass, 0));

            //normalize air falling sound
            m_AudioSrc.pitch = (m_PlayerBody.velocity.y / 100) * 2.0f;              
            return;
        }

        m_AudioSrc.pitch = Mathf.Lerp(m_AudioSrc.pitch, 0.0f, 2.0f * Time.deltaTime);
        m_JumpMovementFactor = 1.0f;

        if (Input.GetButton("Jump"))
        {
            float jumpForce = CalculateVerticalSpeed();
            m_PlayerBody.velocity = new Vector3(velocity.x, jumpForce , velocity.z);
        }
    }

    float CalculateVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpMultiplier * Mathf.Abs(Physics.gravity.y));
    }

    public void ToggleJumpAvailability(bool value)
    {
        jumpMultiplier = value ? m_InitialJumpForce : 0.0f;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    public void ToggleCarryObject(GameObject CarriedObj, bool value)
    {
        if (CarriedObj == null)
            return;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        if (!value)
        {
            CarriedObj.transform.parent = null;
            collider.radius = 0.5f;
            collider.center = Vector3.zero;
        }
        else
        {
            CarriedObj.transform.parent = transform;
            CarriedObj.transform.position = transform.position + Center + PlayerBody.transform.forward;
            Vector3 newCenter = CarriedObj.transform.localPosition / 2.0f;
            newCenter.y = 0.0f;
            collider.center = newCenter;
            collider.radius = Vector3.Distance(CarriedObj.transform.position, transform.position);
        }
    }
}
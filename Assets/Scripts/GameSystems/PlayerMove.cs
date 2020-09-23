using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
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

    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;
    private float m_JumpMovementFactor;

    private Rigidbody m_PlayerBody;
    private float m_InitialJumpForce;

    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private void Awake()
    {
        m_InitialJumpForce = jumpMultiplier;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * (movementSpeed * m_JumpMovementFactor);
        float vertInput = Input.GetAxis(verticalInputName) * (movementSpeed * m_JumpMovementFactor);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        JumpInput();

        float yVel = m_PlayerBody.velocity.y;
        m_PlayerBody.velocity = (forwardMovement + rightMovement) + (Vector3.up * yVel);
    }

    private void JumpInput()
    {
        if (!IsGrounded())
            return;

        m_JumpMovementFactor = 1.0f;
        Debug.Log("BeforeInput");
        if (Input.GetKey(jumpKey))
        {
            Debug.Log("Jump");
            m_PlayerBody.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
            m_JumpMovementFactor = 0.5f;
        }
    }

    public void ToggleJumpAvailability(bool value)
    {
        jumpMultiplier = value ? m_InitialJumpForce : 0.0f;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }
}
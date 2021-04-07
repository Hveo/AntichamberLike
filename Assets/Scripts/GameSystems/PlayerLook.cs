using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;

    private float xAxisClamp;
    private IInteractible m_CurrentSelection;
    private InputAction m_Interact;
    private InputAction m_Look;
    private PlayerPrefsObject m_PlayerPrefs;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;

        layerMask = LayerMask.NameToLayer("Interact");

        InputActionAsset actions = InputHandler.Inputs.actions;
        m_Interact = actions["inputs.interact"];
        m_Look = actions["inputs.look"];

        m_Interact.performed += (context) =>
        {
            if (m_CurrentSelection)
                m_CurrentSelection.Interact();
        };

        m_PlayerPrefs = Core.instance.PlayerPrefs;
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    private RaycastHit hit;
    private int layerMask;

    private void FixedUpdate()
    {
        if (m_CurrentSelection != null && m_CurrentSelection.KeepInteractability)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, 3.0f, 1 << layerMask, QueryTriggerInteraction.Ignore))
        {
            IInteractible interactible = hit.transform.GetComponent<IInteractible>();
            
            if (interactible != null)
            {
                if (interactible != m_CurrentSelection)
                {
                    if (m_CurrentSelection != null)
                        m_CurrentSelection.OnStopBeingInteractible();

                    m_CurrentSelection = interactible;
                    interactible.OnBeingInteractible();
                }
            }
        }
        else if (m_CurrentSelection != null && !m_CurrentSelection.KeepInteractability)
        {
            if (m_CurrentSelection != null)
                m_CurrentSelection.OnStopBeingInteractible();

            m_CurrentSelection = null;
        }
    }

    private void CameraRotation()
    {
        Vector2 cameraLook = m_Look.ReadValue<Vector2>();
        float sensitivity = 0.0f;
        float xAxis = 1.0f;
        float yAxis = 1.0f;

        if (InputHandler.PCLayout)
            sensitivity = m_PlayerPrefs.MouseSensitivity;
        else
        {
            sensitivity = m_PlayerPrefs.StickSensitivity;

            if (m_PlayerPrefs.InvertXAxis)
                xAxis = -1.0f;

            if (m_PlayerPrefs.InvertYAxis)
                yAxis = -1.0f;
        }

        float mouseX = xAxis * cameraLook.x * sensitivity * Time.smoothDeltaTime;
        float mouseY = yAxis * cameraLook.y * sensitivity * Time.smoothDeltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
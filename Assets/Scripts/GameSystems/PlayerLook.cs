using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;
    private IInteracitble m_CurrentSelection;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;

        layerMask = LayerMask.NameToLayer("Interact");
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();

        if (m_CurrentSelection)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Interact")))
            {
                m_CurrentSelection.Interact();
            }
        }
    }

    private RaycastHit hit;
    private int layerMask;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 2.0f, 1 << layerMask, QueryTriggerInteraction.Ignore))
        {
            IInteracitble interactible = hit.transform.GetComponent<IInteracitble>();
            
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
        else
        {
            if (m_CurrentSelection != null)
                m_CurrentSelection.OnStopBeingInteractible();

            m_CurrentSelection = null;
        }
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

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
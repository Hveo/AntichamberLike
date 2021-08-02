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
    private Vector3 m_PlayerCenter;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;

        layerMask = LayerMask.GetMask("Interact", "BlockInteract");

        InputActionAsset actions = InputHandler.Inputs.actions;
        m_Interact = actions["inputs.interact"];
        m_Look = actions["inputs.look"];

        m_Interact.performed += (context) =>
        {
            if (m_CurrentSelection && !UISystem.MenuPresence)
                m_CurrentSelection.Interact();
        };

        m_PlayerCenter = playerBody.GetComponent<PlayerControl>().Center;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!UISystem.MenuPresence)
            CameraRotation();
    }

    private RaycastHit hit;
    private int layerMask;

    private void FixedUpdate()
    {
        if (UISystem.MenuPresence)
            return;
     
        bool isCarryingObject = m_CurrentSelection != null && m_CurrentSelection.KeepInteractability;

        Ray ray;
        if (InputHandler.PCLayout)
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        else
            ray = new Ray(transform.position + m_PlayerCenter, transform.forward);

        if (Physics.Raycast(ray, out hit, 3.0f, layerMask, QueryTriggerInteraction.Collide))
        {
            IInteractible interactible = hit.transform.GetComponent<IInteractible>();

            if (interactible == null)
            {
                if (!isCarryingObject)
                {
                    if (m_CurrentSelection != null)
                        m_CurrentSelection.OnStopBeingInteractible();

                    m_CurrentSelection = null;
                    InterfaceUtilities.Clear();
                }
                return;
            }

            if (isCarryingObject)
            {
                if (interactible is PhysicBox && hit.normal == Vector3.up)
                    GameUtilities.DisplayBoxHelper(interactible.transform.position + Vector3.up, interactible.transform.rotation);

                return;
            }

            if (!interactible.IsInteractible)
                return;
            else if (interactible != m_CurrentSelection)
            {
                if (m_CurrentSelection != null)
                    m_CurrentSelection.OnStopBeingInteractible();

                m_CurrentSelection = interactible;
                interactible.OnBeingInteractible();
                InterfaceUtilities.DisplayAction("inputs.interact");

                GameUtilities.HideBoxHelper();
            }
        }
        else if (m_CurrentSelection != null && !m_CurrentSelection.KeepInteractability)
        {
            if (m_CurrentSelection != null)
                m_CurrentSelection.OnStopBeingInteractible();

            m_CurrentSelection = null;
            InterfaceUtilities.Clear();
        }
        else
            GameUtilities.HideBoxHelper();
    }

    private void CameraRotation()
    {
        PlayerPrefsObject playerPrefs = Core.instance.PlayerPrefs;
        Vector2 cameraLook = m_Look.ReadValue<Vector2>();
        float sensitivity = 1.0f;
        float xAxis = 1.0f;
        float yAxis = 1.0f;

        if (InputHandler.PCLayout)
            sensitivity = playerPrefs.MouseSensitivity;
        else
        {
            sensitivity = playerPrefs.StickSensitivity;

            if (playerPrefs.InvertXAxis)
                xAxis = -1.0f;

            if (playerPrefs.InvertYAxis)
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
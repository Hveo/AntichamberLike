using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MultipleChoiceSelectable : UnityEngine.UI.Selectable
{
    public UnityEngine.UI.Image PreviousIcon;
    public UnityEngine.UI.Image NextIcon;
    public InputActionReference PreviousInput;
    public InputActionReference NextInput;

    System.Action<InputAction.CallbackContext> m_PrevAction;
    System.Action<InputAction.CallbackContext> m_NextAction;

    public bool IsSelected { get; private set; }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InputHandler.onInputDeviceChangedDelegate += SetLayoutDisplay;
        FeedButtonEvents(PreviousIcon.GetComponent<EventTrigger>());
        FeedButtonEvents(NextIcon.GetComponent<EventTrigger>());
        SetLayoutDisplay();
    }

    public void FeedPreviousPressed(System.Action<InputAction.CallbackContext> action)
    {
        m_PrevAction = action;
        PreviousInput.action.performed += action;
        PreviousInput.action.Enable();
    }

    public void FeedNextPressed(System.Action<InputAction.CallbackContext> action)
    {
        m_NextAction = action;
        NextInput.action.performed += action;
        NextInput.action.Enable();
    }

    void FeedButtonEvents(EventTrigger trigg)
    {
        if (trigg == null)
            return;

        GameObject obj = trigg.gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((PointerEventData) =>
        {
            AudioMgr.PlayUISound("Select");
            UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
        });

        trigg.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((PointerEventData) =>
        {
            UIGraphicUtilities.DeselectButton(obj.GetComponent<UnityEngine.UI.Button>());
        });

        trigg.triggers.Add(entry2);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        UIGraphicUtilities.SelectMultipleChoiceSelectable(this);
        PreviousIcon.enabled = true;
        NextIcon.enabled = true;
        IsSelected = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        UIGraphicUtilities.DeselectMultipleChoiceSelectable(this);

        if (!InputHandler.PCLayout)
        {
            PreviousIcon.enabled = false;
            NextIcon.enabled = false;
        }

        IsSelected = false;
    }

    void SetLayoutDisplay()
    {
        NextIcon.transform.rotation = InputHandler.PCLayout ? Quaternion.Euler(0.0f, 0.0f, -90.0f) : Quaternion.identity;
        PreviousIcon.transform.rotation = InputHandler.PCLayout ? Quaternion.Euler(0.0f, 0.0f, 90.0f) : Quaternion.identity;
        NextIcon.enabled = (InputHandler.PCLayout || IsSelected);
        PreviousIcon.enabled = (InputHandler.PCLayout || IsSelected);
    }

    protected override void OnDestroy()
    {
        PreviousInput.action.performed -= m_PrevAction;
        NextInput.action.performed -= m_NextAction;
        InputHandler.onInputDeviceChangedDelegate -= SetLayoutDisplay;
        base.OnDestroy();
    }
}

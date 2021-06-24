using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Security.AccessControl;

public class InputMapperWindow : MonoBehaviour, IUIWindows
{
    public ActionRebindSlot ActionRebindTemplate;
    public GameObject WaitingRebindWindow;
    public GameObject SwitchStickLayout;
    public InputActionRebindingExtensions.RebindingOperation CurrentRebindOperation;
    public TextMeshProUGUI DeviceName;
    public Selectable[] Selectables;

    private GameObject m_SlotLayout;
    private CanvasGroup m_CanvasGroup;
    private ActionRebindSlot m_CurrentSlot;
    private bool m_ChangingLayout;

    void Awake()
    {
        m_SlotLayout = ActionRebindTemplate.transform.parent.gameObject;
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_ChangingLayout = false;
        DeviceName.text = InputHandler.DeviceName;
        UISystem.instance.StickPressInput.action.performed += SwitchSticksInput;
        UISystem.instance.StickPressInput.action.Enable();
        SwitchStickLayout.SetActive(!InputHandler.PCLayout);
    }

    // Start is called before the first frame update
    void Start()
    {
        InputHandler.DisableInputs();
        FeedUIElementsWithEvents();
        FeedInputTable();
        InputHandler.onInputDeviceChangedDelegate += OnDeviceChanged;
    }

    public void FeedUIElementsWithEvents()
    {
        for (int i = 0; i < Selectables.Length; ++i)
        {
            EventTrigger trigg = Selectables[i].GetComponent<EventTrigger>();

            if (!(trigg is null))
            {
                GameObject obj = Selectables[i].gameObject;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnSelect(PointerEventData);
                });

                trigg.triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.Select;
                entry2.callback.AddListener((PointerEventData) =>
                {
                    if (!(obj.GetComponent<UnityEngine.UI.Button>() is null))
                        UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry2);

                EventTrigger.Entry entry3 = new EventTrigger.Entry();
                entry3.eventID = EventTriggerType.PointerExit;
                entry3.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnDeselect(PointerEventData);
                });

                trigg.triggers.Add(entry3);

                EventTrigger.Entry entry4 = new EventTrigger.Entry();
                entry4.eventID = EventTriggerType.Deselect;
                entry4.callback.AddListener((PointerEventData) =>
                {
                    if (!(obj.GetComponent<UnityEngine.UI.Button>() is null))
                        UIGraphicUtilities.DeselectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry4);
            }
        }
    }

    void SetEventOnBindButton(EventTrigger butTrigg)
    {
        if (butTrigg is null)
            return;

        GameObject obj = butTrigg.gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((PointerEventData) =>
        {
            butTrigg.OnSelect(PointerEventData);
        });

        butTrigg.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Select;
        entry2.callback.AddListener((PointerEventData) =>
        {
            if (!(obj.GetComponent<UnityEngine.UI.Button>() is null))
                LeanTween.scale(obj, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        });

        butTrigg.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((PointerEventData) =>
        {
            butTrigg.OnDeselect(PointerEventData);
        });

        butTrigg.triggers.Add(entry3);

        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.Deselect;
        entry4.callback.AddListener((PointerEventData) =>
        {
            if (!(obj.GetComponent<UnityEngine.UI.Button>() is null))
                LeanTween.scale(obj, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
        });

        butTrigg.triggers.Add(entry4);
    }

    public void SetDefaultItemSelected()
    {
        if (m_SlotLayout.transform.childCount > 1)
        {
            if (InputHandler.PCLayout)
                UISystem.instance.SelectItem(m_SlotLayout.transform.GetChild(1).GetComponent<ActionRebindSlot>().ActionRebindButton.gameObject);
            else
                UISystem.instance.SelectItem(m_SlotLayout.transform.GetChild(3).GetComponent<ActionRebindSlot>().ActionRebindButton.gameObject);
        }
    }

    public void OnCancelInputPressed()
    {
        OnClickBackButton();
    }

    public void OnClickBackButton()
    {
        AudioMgr.PlayUISound("Cancel");
        UISystem.instance.CloseCurrentWindow();
        Resources.UnloadUnusedAssets();
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }

    #region InputBinding

    void FeedInputTable()
    {
        InputHandler.InputBindingInfo[] ActionIndex = InputHandler.GetInputsForCurrentDevice();
        Dictionary<string, int> InputSlots = InputHandler.PCLayout ? new Dictionary<string, int>() : null; //PC Layout allow player to map 2 inputs to one action
        int childcount = 1;

        for (int i = 0; i < ActionIndex.Length; ++i)
        {
            InputHandler.InputBindingInfo act = ActionIndex[i];
            ActionRebindSlot slot;
            string actName = act.Action.name;
            bool lockRebind = false;

            //No rebind allowed for camera
            if (string.CompareOrdinal(act.Action.expectedControlType, "Vector2") == 0)
            {
                if (act.ControlPath.Contains("Mouse"))
                    continue;

                lockRebind = true;

                if (InputHandler.PCLayout && act.IsComposite)
                {
                    actName = TryDisassembleCompositeAction(act.Action, act.BindIndex);
                    lockRebind = false;
                }
            }
        
            if (InputHandler.PCLayout && InputSlots.ContainsKey(actName))
            {
                slot = GetSlotAt(InputSlots[actName]);
                slot.AltPath = act.ControlPath;
                slot.SetAltIcon();
                SetEventOnBindButton(slot.AltRebindButton.GetComponent<EventTrigger>());
            }
            else
            {
                slot = GameObject.Instantiate(ActionRebindTemplate.gameObject, m_SlotLayout.transform).GetComponent<ActionRebindSlot>();
                slot.ActionLabel.text = LocalizationSystem.GetEntry(actName.ToLower());
                slot.ActionPath = act.ControlPath;
                slot.SetInputIcon();

                if (lockRebind)
                    slot.LockBind();

                if (InputHandler.PCLayout)
                {
                    InputSlots.Add(actName, childcount);
                    childcount++;
                }

                SetEventOnBindButton(slot.ActionRebindButton.GetComponent<EventTrigger>());
            }

            slot.ID = act.Action.id.ToString();
            slot.SetIndex(act.BindIndex);
            slot.gameObject.SetActive(true);
        }

        SetNavigation();
        SetDefaultItemSelected();
    }

    //One of the most terrible function to watch
    void SetNavigation()
    {
        ActionRebindSlot[] slots = m_SlotLayout.GetComponentsInChildren<ActionRebindSlot>(false);

        for (int i = 1; i < slots.Length; ++i)
        {
            Selectable btn = slots[i].ActionRebindButton;
            Selectable prevBtn = slots[i - 1].ActionRebindButton;
            Navigation prevNavigation = prevBtn.navigation;

            if (i != 1)
                prevNavigation.selectOnUp = slots[i - 2].ActionRebindButton;
            
            prevNavigation.selectOnDown = btn;
            prevBtn.navigation = prevNavigation;

            if (!InputHandler.PCLayout)
                continue;

            //Same for alt rebind
            btn = slots[i].AltRebindButton;
            prevBtn = slots[i - 1].AltRebindButton;
            prevNavigation = prevBtn.navigation;

            if (i != 1)
                prevNavigation.selectOnUp = slots[i - 2].AltRebindButton;

            prevNavigation.selectOnDown = btn;
            prevBtn.navigation = prevNavigation;
        }

        Selectable lastBtn = slots[slots.Length - 1].ActionRebindButton;
        Navigation nav = lastBtn.navigation;
        nav.selectOnUp = slots[slots.Length - 2].ActionRebindButton;
        lastBtn.navigation = nav;

        Selectable BackBtn = Selectables[2];
        nav = BackBtn.navigation;

        if (!InputHandler.PCLayout)
            nav.selectOnDown = slots[2].ActionRebindButton;
        else
            nav.selectOnDown = slots[0].ActionRebindButton;
        
        nav.selectOnUp = lastBtn;
        BackBtn.navigation = nav;
        
        if (InputHandler.PCLayout)
        {
            lastBtn = slots[slots.Length - 1].AltRebindButton;
            nav = lastBtn.navigation;
            nav.selectOnUp = slots[slots.Length - 2].AltRebindButton;
            lastBtn.navigation = nav;
        }
    }

    void SwitchSticksInput(InputAction.CallbackContext ctx)
    {
        if (InputHandler.PCLayout)
            return;

        ActionRebindSlot slot1 = GetSlotAt(1);
        ActionRebindSlot slot2 = GetSlotAt(2);

        InputActionMap map = InputHandler.Inputs.actions.actionMaps[0]; //The PlayerInput map 

        int index1 = FindRealBinding(slot1.ActionPath);
        int index2 = FindRealBinding(slot2.ActionPath);

        string path1 = slot1.ActionPath;
        string path2 = slot2.ActionPath;
        map.ApplyBindingOverride(index1, new InputBinding { overridePath = path2 });
        map.ApplyBindingOverride(index2, new InputBinding { overridePath = path1 });

        slot1.ActionPath = path2;
        slot2.ActionPath = path1;
        slot1.SetInputIcon();
        slot2.SetInputIcon();
    }

    /// <summary>
    /// Since we have stored bindIndex using the New input system hashmap, we don't have the real binding index in the map binding set
    /// </summary>
    /// <param name="path"></param>
    int FindRealBinding(string path)
    {
        for (int i = 0; i < InputHandler.Inputs.actions.actionMaps[0].bindings.Count; ++i)
        {
            if (InputHandler.Inputs.actions.actionMaps[0].bindings[i].effectivePath == path)
                return i;
        }

        return -1;
    }

    string TryDisassembleCompositeAction(InputAction act, int index)
    {
        return act.name + act.bindings[index].name;
    }

    ActionRebindSlot GetSlotAt(int idx)
    {
        if (idx < 1 && idx > m_SlotLayout.transform.childCount)
            return null;

        return m_SlotLayout.transform.GetChild(idx).GetComponent<ActionRebindSlot>();
    }

    public void RemapInput(ActionRebindSlot slot)
    {
        m_CurrentSlot = slot;
        MapControl(InputHandler.Inputs.actions.FindAction(slot.ID), slot.BindIndex, false);
    }

    public void RemapAltInput(ActionRebindSlot slot)
    {
        m_CurrentSlot = slot;
        MapControl(InputHandler.Inputs.actions.FindAction(slot.ID), slot.AltIndex, true);
    }

    public void MapControl(InputAction action, int bindingIndex, bool altAction)
    {
        if (bindingIndex == -1 || action is null || m_CurrentSlot is null)
            return;

        PerformInteractiveRebind(action, bindingIndex, altAction);
    }

    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool altAction, bool allCompositeParts = false)
    {
        CurrentRebindOperation?.Cancel();
        m_CanvasGroup.interactable = false;
        WaitingRebindWindow.SetActive(true);

        CurrentRebindOperation = action.PerformInteractiveRebinding(bindingIndex).OnCancel((operation) =>
        {
            CurrentRebindOperation?.Dispose();
            CurrentRebindOperation = null;

            //Hide Rebind UI
            m_CurrentSlot = null;
            m_CanvasGroup.interactable = true;
            WaitingRebindWindow.SetActive(false);

        }).OnComplete((operation) =>
        {
            CurrentRebindOperation?.Dispose();
            CurrentRebindOperation = null;


            if (altAction)
            {
                m_CurrentSlot.AltPath = action.bindings[bindingIndex].effectivePath;
                m_CurrentSlot.SetAltIcon();
            }
            else
            {
                m_CurrentSlot.ActionPath = action.bindings[bindingIndex].effectivePath;
                m_CurrentSlot.SetInputIcon();
            }

            m_CanvasGroup.interactable = true;
            WaitingRebindWindow.SetActive(false);

        });

        if (InputHandler.PCLayout)
            CurrentRebindOperation.WithCancelingThrough("<Keyboard>/escape");
        else
        {
            CurrentRebindOperation.WithCancelingThrough("<Gamepad>/start");
            CurrentRebindOperation.WithControlsExcluding("<Gamepad>/rightStick");
            CurrentRebindOperation.WithControlsExcluding("<Gamepad>/leftStick");
            CurrentRebindOperation.WithControlsExcluding("<DualShockGamepad>/touchpadButton");
        }

        CurrentRebindOperation.Start();
    }
    

    void OnDeviceChanged()
    {
        if (WaitingRebindWindow.activeSelf || m_ChangingLayout)
            return;

        if (InputHandler.PCLayout)
            UISystem.instance.StickPressInput.action.Disable();
        else
            UISystem.instance.StickPressInput.action.Enable();

        m_ChangingLayout = true;
        ClearInputTable();
        StartCoroutine(DelayFeedTable());
    }

    ///This is just an antispam method because UI cannot be destroyed and recreated directly
    IEnumerator DelayFeedTable()
    {
        yield return null;
        FeedInputTable();
        DeviceName.text = InputHandler.DeviceName;
        m_ChangingLayout = false;
        SwitchStickLayout.SetActive(!InputHandler.PCLayout);
    }

    void ClearInputTable()
    {
        for (int i = m_SlotLayout.transform.childCount - 1; i > 0 ; --i)
        {
            if (m_SlotLayout.transform.GetChild(i))
                Destroy(m_SlotLayout.transform.GetChild(i).gameObject);
        }
    }

    #endregion

    void OnDestroy()
    {
        InputHandler.EnableInputs();
        InputHandler.onInputDeviceChangedDelegate -= OnDeviceChanged;
        UISystem.instance.StickPressInput.action.performed -= SwitchSticksInput;
        UISystem.instance.StickPressInput.action.Disable();
        InputHandler.SaveInputPrefs();
    }
}

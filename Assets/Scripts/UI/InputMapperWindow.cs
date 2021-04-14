using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.iOS;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputMapperWindow : MonoBehaviour, IUIWindows
{
    public ActionRebindSlot ActionRebindTemplate;
    public GameObject WaitingRebindWindow;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        InputHandler.DisableInputs();
        FeedUIElementsWithEvents();
        FeedInputTable();
        InputHandler.onInputDeviceChangedDelegate += OnDeviceChanged;
    }

    public bool IsPersistant()
    {
        return false;
    }

    public void FeedUIElementsWithEvents()
    {
        for (int i = 0; i < Selectables.Length; ++i)
        {
            EventTrigger trigg = Selectables[i].GetComponent<EventTrigger>();

            if (trigg != null)
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
                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
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
                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
                        UIGraphicUtilities.DeselectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry4);
            }
        }
    }

    public void SetDefaultItemSelected()
    {
        if (m_SlotLayout.transform.childCount > 1)
            UISystem.instance.SelectItem(m_SlotLayout.transform.GetChild(1).GetComponent<ActionRebindSlot>().ActionRebindButton.gameObject);
    }

    public void OnClickBackButton()
    {
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
                break;

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
    }
}

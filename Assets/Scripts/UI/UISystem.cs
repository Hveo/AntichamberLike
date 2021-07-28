using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public struct WindowSelectable
    {
        public IUIWindows Window;
        public GameObject ObjectSelected;
    }

    public static UISystem instance;
    public IUIWindows WindowFocused { get; private set; }
    public static bool MenuPresence { get; private set; }
    public InputActionReference CloseWindowAction;
    public InputActionReference TogglePauseAction;
    public InputActionReference StickPressInput;

    public delegate void onSelectionChange(GameObject obj);
    public onSelectionChange onSelectionChangeEvent;

    private Stack<WindowSelectable> m_WindowStack;
    private GameObject m_ConfirmPopup;
    private GameObject m_ConfirmPopupTimed;
    private GameObject m_PauseWindow;
    private GameObject m_PrevSelection;
    private Texture2D m_NoCursorRes;

    private IEnumerator Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        StartCoroutine(InterfaceUtilities.InitResources());

        m_WindowStack = new Stack<WindowSelectable>();
        ResourceRequest req = Resources.LoadAsync("UI/ConfirmPopup");

        while (!req.isDone)
            yield return null;

        m_ConfirmPopup = req.asset as GameObject;

        req = Resources.LoadAsync("UI/ConfirmPopupTimed");

        while (!req.isDone)
            yield return null;

        m_ConfirmPopupTimed = req.asset as GameObject;

        req = Resources.LoadAsync("UI/PauseMenu");

        while (!req.isDone)
            yield return null;

        m_PauseWindow = req.asset as GameObject;
        CloseWindowAction.action.performed += CancelInputPressed;
        TogglePauseAction.action.performed += PauseInputPressed;
        
        req = Resources.LoadAsync("Inputs/NoCursorTex");

        while (!req.isDone)
            yield return null;

        m_NoCursorRes = req.asset as Texture2D;
    }

    public GameObject CurrentSelection
    {
        get { return EventSystem.current.currentSelectedGameObject; }
        private set { }
    }

    public void LockUnlockPauseAction(bool activate)
    {
        if (activate)
            TogglePauseAction.action.Enable();
        else
            TogglePauseAction.action.Disable();
    }

    public void SetMenuPresence(bool value)
    {
        MenuPresence = value;

        if (!MenuPresence)
        {
            CloseWindowAction.action.Disable();
            Cursor.SetCursor(m_NoCursorRes, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            CloseWindowAction.action.Enable();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SelectItem(GameObject item)
    {
        if (CurrentSelection == item)
            return;

        EventTrigger trigg = item.GetComponent<EventTrigger>();

        if (trigg != null)
            trigg.OnDeselect(new PointerEventData(EventSystem.current));

        EventSystem.current.SetSelectedGameObject(item);
        trigg = item.GetComponent<EventTrigger>();

        if (trigg != null)
            trigg.OnPointerEnter(new PointerEventData(EventSystem.current));
    }

    public void NewFocusedWindow(GameObject windowObj, bool stack = false, bool persistantWindow = false)
    {
        IUIWindows window = windowObj.GetComponent<IUIWindows>();

        if (window == null)
            return;

        if (stack)
        {
            WindowSelectable WS;

            WS.Window = WindowFocused;
            WS.ObjectSelected = CurrentSelection;

            m_WindowStack.Push(WS);
        }

        ToggleWindowInteractable(WindowFocused?.GetWindowObject(), false);
        WindowFocused = window;
        ToggleWindowInteractable(windowObj, true);
        window.SetDefaultItemSelected();
    }

    public void ClearAll()
    {
        m_WindowStack.Clear();
        WindowFocused = null;
        CurrentSelection = null;
    }

    public void CancelInputPressed(InputAction.CallbackContext ctx)
    {
        if (WindowFocused != null)
            WindowFocused.OnCancelInputPressed();
    }

    void PauseInputPressed(InputAction.CallbackContext ctx)
    {
        if (MenuPresence)
        {
            if (WindowFocused is PauseMenuWindow)
                (WindowFocused as PauseMenuWindow).Resume();
        }
        else
            NewFocusedWindow(GameObject.Instantiate(m_PauseWindow), true);
    }

    public void CloseWindow(GameObject window)
    {
        if (window is null)
            return;

        Destroy(window);

        if (!(m_WindowStack is null) && m_WindowStack.Count > 0)
        {
            WindowSelectable WS = m_WindowStack.Pop();

            if (WS.Window != null)
            {
                WindowFocused = WS.Window;
                SelectItem(WS.ObjectSelected);
                ToggleWindowInteractable(WS.Window.GetWindowObject(), true);
                return;
            }
        }

        SetMenuPresence(false);
    }

    public void CloseCurrentWindow()
    {
        GameObject window = WindowFocused.GetWindowObject();
        CloseWindow(window);
    }


    public void ToggleWindowInteractable(GameObject window, bool value)
    {
        if (window != null)
        {
            CanvasGroup group = window.GetComponentInChildren<CanvasGroup>();

            if (group != null)
                group.interactable = value;
        }
    }

    public void Update()
    {
        if (!MenuPresence)
            return;
        
        if (WindowFocused != null)
        {
            if (CurrentSelection == null)
                WindowFocused.SetDefaultItemSelected();

            if (m_PrevSelection != CurrentSelection)
            {
                onSelectionChangeEvent?.Invoke(CurrentSelection);
            }

            m_PrevSelection = CurrentSelection;
        }      
    }

    public void ToggleConfirmExit()
    {
        CreatePopup(LocalizationSystem.GetEntry("menu.confirm"), "menu.yes", "menu.no", () => { Application.Quit(); }, () => { AudioMgr.PlayUISound("Cancel");  CloseCurrentWindow(); });      
    }

    public void CreatePopup(string contentKey, string buttonLeftKey, string buttonRightKey, UnityAction leftAction, UnityAction rightAction, string titleKey = "")
    {
        AudioMgr.PlayUISound("Confirm");
        GameObject ConfirmPopupObj = GameObject.Instantiate(m_ConfirmPopup);
        NewFocusedWindow(ConfirmPopupObj, true);
        ConfirmPopupObj.GetComponent<ConfirmBoxGeneric>().BuildBox(contentKey, buttonLeftKey, buttonRightKey, leftAction, rightAction, titleKey);
    }

    public void CreateTimedPopup(string contentKey, string buttonLeftKey, string buttonRightKey, UnityAction leftAction, UnityAction rightAction, float Timer, string titleKey = "")
    {
        AudioMgr.PlayUISound("Confirm");
        GameObject ConfirmPopupObj = GameObject.Instantiate(m_ConfirmPopupTimed);
        NewFocusedWindow(ConfirmPopupObj, true);
        ConfirmBoxTimed box = ConfirmPopupObj.GetComponent<ConfirmBoxTimed>();
        box.BuildBox(contentKey, buttonLeftKey, buttonRightKey, leftAction, rightAction, titleKey);
        box.StartTimer(Timer);
    }
}

public interface IUIWindows
{
    void SetDefaultItemSelected();
    void OnCancelInputPressed();
    void FeedUIElementsWithEvents();

    GameObject GetWindowObject();
}

public static class UIGraphicUtilities
{
    static Color SelectedColor = new Color(1.0f, 0.5f, 0.0f);

    public static void SelectButton(UnityEngine.UI.Button button)
    {
        LeanTween.scale(button.gameObject, new Vector3(1.4f, 1.4f, 1.4f), 0.2f);
        TextMeshProUGUI txtMesh = button.GetComponentInChildren<TextMeshProUGUI>();

        if (txtMesh != null)
            txtMesh.color = SelectedColor;
    }

    public static void DeselectButton(UnityEngine.UI.Button button)
    {
        LeanTween.scale(button.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
        TextMeshProUGUI txtMesh = button.GetComponentInChildren<TextMeshProUGUI>();

        if (txtMesh != null)
            txtMesh.color = Color.black;
    }

    public static void SelectSlider(UnityEngine.UI.Slider slider)
    {
        slider.handleRect.GetComponent<Image>().color = Color.black;
        LeanTween.scale(slider.handleRect.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        slider.fillRect.GetComponent<Image>().color = SelectedColor;
    }

    public static void DeselectSlider(UnityEngine.UI.Slider slider)
    {
        slider.handleRect.GetComponent<Image>().color = Color.white;
        LeanTween.scale(slider.handleRect.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
        slider.fillRect.GetComponent<Image>().color = Color.black;
    }

    public static void SelectToggle(UnityEngine.UI.Toggle toggle)
    {
        toggle.targetGraphic.color = SelectedColor;
        LeanTween.scale(toggle.targetGraphic.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
    }

    public static void DeselectToggle(UnityEngine.UI.Toggle toggle)
    {
        toggle.targetGraphic.color = Color.white;
        LeanTween.scale(toggle.targetGraphic.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
    }

    public static void SelectMultipleChoiceSelectable(MultipleChoiceSelectable mcs)
    {
        LeanTween.scale(mcs.targetGraphic.gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        TextMeshProUGUI txtMesh = mcs.GetComponentInChildren<TextMeshProUGUI>();

        if (txtMesh != null)
            txtMesh.color = SelectedColor;
    }

    public static void DeselectMultipleChoiceSelectable(MultipleChoiceSelectable mcs)
    {
        LeanTween.scale(mcs.targetGraphic.gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        TextMeshProUGUI txtMesh = mcs.GetComponentInChildren<TextMeshProUGUI>();

        if (txtMesh != null)
            txtMesh.color = Color.black;
    }
}
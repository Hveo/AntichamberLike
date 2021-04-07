using System.Collections;
using System.Collections.Generic;
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
    public bool MenuPresence { get; private set; }
    public InputActionReference CloseWindowAction;
    
    private Stack<WindowSelectable> m_WindowStack;
    private GameObject m_ConfirmPopup;

    private IEnumerator Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        m_WindowStack = new Stack<WindowSelectable>();
        ResourceRequest req = Resources.LoadAsync("UI/ConfirmPopup");

        while (!req.isDone)
            yield return null;

        m_ConfirmPopup = req.asset as GameObject;

        CloseWindowAction.action.performed += CloseWindowWithInput;
        CloseWindowAction.action.Enable();
    }

    public GameObject CurrentSelection
    {
        get { return EventSystem.current.currentSelectedGameObject; }
        private set { }
    }

    public void SetMenuPresence(bool value)
    {
        MenuPresence = value;

        if (!MenuPresence)
            CloseWindowAction.action.performed -= CloseWindowWithInput;
    }

    public void SelectItem(GameObject item)
    {
        if (CurrentSelection == item)
            return;

        EventSystem.current.SetSelectedGameObject(item);
        EventTrigger trigg = item.GetComponent<EventTrigger>();

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

    public void CloseWindowWithInput(InputAction.CallbackContext ctx)
    {
        CloseCurrentWindow();
    }

    public void CloseWindow(GameObject window)
    {
        if (window is null)
            return;

        if (WindowFocused.IsPersistant())
            return;

        Destroy(window);

        if (!(m_WindowStack is null) && m_WindowStack.Count > 0)
        {
            WindowSelectable WS = m_WindowStack.Pop();

            WindowFocused = WS.Window;
            SelectItem(WS.ObjectSelected);
            ToggleWindowInteractable(WS.Window.GetWindowObject(), true);

            return;
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

        if (!(WindowFocused is null))
        {
            if (CurrentSelection is null)
                WindowFocused.SetDefaultItemSelected();
            
            Debug.Log(CurrentSelection);
        }      
    }

    public void ToggleConfirmExit()
    {
        CreatePopup("menu.confirm", "menu.yes", "menu.no", () => { Application.Quit(); }, () => { CloseCurrentWindow(); });      
    }

    public void CreatePopup(string contentKey, string buttonLeftKey, string buttonRightKey, UnityAction leftAction, UnityAction rightAction, string titleKey = "")
    {
        GameObject ConfirmPopupObj = GameObject.Instantiate(m_ConfirmPopup);
        NewFocusedWindow(ConfirmPopupObj, true);
        ConfirmPopupObj.GetComponent<ConfirmBoxGeneric>().BuildBox(contentKey, buttonLeftKey, buttonRightKey, leftAction, rightAction, titleKey);
    }
}

public interface IUIWindows
{
    void SetDefaultItemSelected();
    void FeedUIElementsWithEvents();

    bool IsPersistant();

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

}
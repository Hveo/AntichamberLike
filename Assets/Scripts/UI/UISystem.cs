using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
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
        ResourceRequest req = Resources.LoadAsync("ConfirmQuit");

        while (!req.isDone)
            yield return null;

        m_ConfirmPopup = req.asset as GameObject;
    }

    public GameObject CurrentSelection
    {
        get { return EventSystem.current.currentSelectedGameObject; }
        private set { }
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

    public void NewFocusedWindow(GameObject windowObj, bool Stack = false)
    {
        IUIWindows window = windowObj.GetComponent<IUIWindows>();

        if (window == null)
            return;

        if (Stack)
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

    public void CloseWindow(GameObject window)
    {
        Destroy(window);

        if (m_WindowStack != null && m_WindowStack.Count > 0)
        {
            WindowSelectable WS = m_WindowStack.Pop();

            WindowFocused = WS.Window;
            SelectItem(WS.ObjectSelected);
        }
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
        Debug.Log(CurrentSelection);
    }

    public void ToggleConfirmExit()
    {
        NewFocusedWindow(GameObject.Instantiate(m_ConfirmPopup), true);
    }
}

public interface IUIWindows
{
    void SetDefaultItemSelected();
    void FeedButtonsWithEvents();
    GameObject GetWindowObject();
}
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
        public GameObject Window;
        public GameObject ObjectSelected;
    }

    public static UISystem instance;
    public GameObject WindowFocused { get; private set; }

    private Stack<WindowSelectable> WindowStack;
    private GameObject ConfirmPopup;

    private IEnumerator Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        WindowStack = new Stack<WindowSelectable>();
        ResourceRequest req = Resources.LoadAsync("ConfirmQuit");

        while (!req.isDone)
            yield return null;

        ConfirmPopup = req.asset as GameObject;
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

    public void NewFocusedWindow(GameObject window, bool Stack = false)
    {
        if (Stack)
        {
            WindowSelectable WS;

            WS.Window = WindowFocused;
            WS.ObjectSelected = CurrentSelection;

            WindowStack.Push(WS);
        }

        ToggleWindowInteractable(WindowFocused, false);
        WindowFocused = window;
        ToggleWindowInteractable(WindowFocused, true);
    }

    public void CloseWindow(GameObject window)
    {
        Destroy(window);

        if (WindowStack != null && WindowStack.Count > 0)
        {
            WindowSelectable WS = WindowStack.Pop();

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
        NewFocusedWindow(GameObject.Instantiate(ConfirmPopup), true);
    }
}

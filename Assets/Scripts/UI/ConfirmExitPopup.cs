using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConfirmExitPopup : MonoBehaviour, IUIWindows
{
    public UnityEngine.UI.Selectable[] Selectables;

    void Awake()
    {
        FeedUIElementsWithEvents();
    }

    public void Cancel()
    {
        UISystem.instance.CloseWindow(gameObject);
    }

    public void Exit()
    {
        Application.Quit();
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
                    UISystem.instance.SelectItem(obj);
                });

                trigg.triggers.Add(entry);
            }
        }
    }

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(Selectables[1].gameObject);
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }
}

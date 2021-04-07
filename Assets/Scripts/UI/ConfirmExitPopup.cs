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

    public bool IsPersistant()
    {
        return false;
    }

    public void Cancel()
    {
        UISystem.instance.CloseCurrentWindow();
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
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((PointerEventData) =>
                {
                    UISystem.instance.SelectItem(obj);

                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
                        UIGraphicUtilities.SelectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerEnter;
                entry2.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnSelect(PointerEventData);
                });

                trigg.triggers.Add(entry2);

                EventTrigger.Entry entry3 = new EventTrigger.Entry();
                entry3.eventID = EventTriggerType.Deselect;
                entry3.callback.AddListener((PointerEventData) =>
                {
                    if (obj.GetComponent<UnityEngine.UI.Button>() != null)
                        UIGraphicUtilities.DeselectButton(obj.GetComponent<UnityEngine.UI.Button>());
                });

                trigg.triggers.Add(entry3);

                EventTrigger.Entry entry4 = new EventTrigger.Entry();
                entry4.eventID = EventTriggerType.PointerExit;
                entry4.callback.AddListener((PointerEventData) =>
                {
                    trigg.OnDeselect(PointerEventData);
                });

                trigg.triggers.Add(entry4);
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

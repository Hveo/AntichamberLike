using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AboutWindow : MonoBehaviour, IUIWindows
{
    public Selectable[] Selectables;
    // Start is called before the first frame update
    void Awake()
    {
        FeedButtonsWithEvents();
    }

    public void Cancel()
    {
        UISystem.instance.CloseWindow(gameObject);
    }

    public void OpenLinkedIn()
    {
        Application.OpenURL("https://linkedin.com/in/johann-seys-727a3b84");
    }

    public void OpenPortfolio()
    {
        Application.OpenURL("https://johannseys.wixsite.com/portfolio");
    }

    public void FeedButtonsWithEvents()
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
        UISystem.instance.SelectItem(Selectables[0].gameObject);
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }
}

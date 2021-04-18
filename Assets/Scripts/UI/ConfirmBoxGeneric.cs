using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfirmBoxGeneric : MonoBehaviour, IUIWindows
{
    public UnityEngine.UI.Button[] Selectables;
    public TextReplacer Title;
    public TextReplacer Content;

    private void Awake()
    {
        FeedUIElementsWithEvents();
    }

    public void BuildBox(string contentKey, string LeftButtonKey, string RightButtonKey, UnityAction leftButtonClickEvent, UnityAction rightButtonClickEvent,  string TitleKey = "")
    {
        Title.enabled = false;
        Content.enabled = false;
        Title.LocalizedText.Key = TitleKey;
        Content.LocalizedText.Key = contentKey;

        TextReplacer tmp1 = Selectables[0].GetComponentInChildren<TextReplacer>();
        tmp1.LocalizedText.Key = LeftButtonKey;
        tmp1.enabled = false;
        Selectables[0].onClick.AddListener(leftButtonClickEvent);
        

        TextReplacer tmp2 = Selectables[1].GetComponentInChildren<TextReplacer>();
        tmp2.LocalizedText.Key = RightButtonKey;
        tmp2.enabled = false;
        Selectables[1].onClick.AddListener(rightButtonClickEvent);

        Title.enabled = true;
        Content.enabled = true;
        tmp1.enabled = true;
        tmp2.enabled = true;
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
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((PointerEventData) =>
                {
                    UISystem.instance.SelectItem(obj);
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

    public GameObject GetWindowObject()
    {
        return gameObject;
    }

    public void SetDefaultItemSelected()
    {
        UISystem.instance.SelectItem(Selectables[1].gameObject);
    }

    public void OnCancelInputPressed()
    {

    }
}

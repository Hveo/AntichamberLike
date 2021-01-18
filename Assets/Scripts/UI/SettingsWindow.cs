using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, IUIWindows
{
    bool m_ValueChanged;

    public Selectable[] Selectables;

    void Awake()
    {
        FeedUIElementsWithEvents();
    }

    public void FeedUIElementsWithEvents()
    {
        
    }

    public GameObject GetWindowObject()
    {
        return gameObject;
    }

    public void SetDefaultItemSelected()
    {
        
    }

    public void Cancel()
    {
        if (!m_ValueChanged)
            UISystem.instance.CloseWindow(gameObject);
    }
}

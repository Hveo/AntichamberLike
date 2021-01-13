using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EventCondition
{
    public int StateValueToTrigger;
    public UnityEvent EventToTrigger;
}

public class EventListener : MonoBehaviour
{
    public bool IgnoreStates;
    public string StateID;
    public EventCondition[] Events;

    protected int m_CurrentValue;

    private void Start()
    {
        if (StateID != string.Empty && Events != null)
            SubscribeToEvent();
    }

    public virtual void SubscribeToEvent()
    {
        LevelMgr.instance.SubscribeToEvent(StateID, this);
    }

    public virtual void OnEventChange()
    {
        if (Events == null)
            return;

        m_CurrentValue = LevelMgr.instance.GetStateValue(StateID);

        for (int i = 0; i < Events.Length; ++i)
        {
            if (Events[i].EventToTrigger != null && m_CurrentValue == Events[i].StateValueToTrigger)
                Events[i].EventToTrigger.Invoke();
        }
    }

    public virtual void ToggleEffectBypass()
    {
        if (!IgnoreStates)
            return;

        for (int i = 0; i < Events.Length; ++i)
        {
            Events[i].EventToTrigger.Invoke();
        }
    }
}

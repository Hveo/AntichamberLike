using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public string StateID;
    
    protected int m_CurrentValue;


    public virtual void SubscribeToEvent()
    {
        GameMgr.instance.SubscribeToEvent(StateID, this);
    }

    public virtual void OnEventChange() { }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

[System.Serializable]
public enum EComparator
{
    LESS,
    LEQUAL,
    EQUAL,
    NOTEQUAL,
    GEQUAL,
    GREATER,
};

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;
    public CharacterController Player;
    public Dictionary<string, int> States;
    public Dictionary<string, List<EventListener>> StatesSubscriber;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        States = new Dictionary<string, int>();
        StatesSubscriber = new Dictionary<string, List<EventListener>>();
    }

    void CreateNewState(string StateID, int value)
    {
        if (States.ContainsKey(StateID))
            return;

        States.Add(StateID, value);
    }

    public int GetStateValue(string StateID)
    {
        if (States.ContainsKey(StateID))
            return States[StateID];

        return -99;
    }

    public void SetStateValue(string StateID, int value)
    {
        if (States.ContainsKey(StateID))
        {
            States[StateID] = value;
            OnEventStateChange(StateID);
        }
    }

    public void IncrementStateValue(string StateID)
    {
        if (States.ContainsKey(StateID))
        {
            States[StateID] += 1;
            OnEventStateChange(StateID);
        }
    }

    public void SubscribeToEvent(string StateID, EventListener ev)
    {
        if (!StatesSubscriber.ContainsKey(StateID))
        {
            StatesSubscriber.Add(StateID, new List<EventListener>() { ev });
            CreateNewState(StateID, 0);
            return;
        }

        if (!StatesSubscriber[StateID].Contains(ev))
        {
            StatesSubscriber[StateID].Add(ev);
        }
    }

    public void OnEventStateChange(string StateID)
    {
        List<EventListener> Listeners = StatesSubscriber[StateID];
        
        for (int i = 0; i < Listeners.Count; ++i)
        {
            Listeners[i].OnEventChange();
        }
    }
}

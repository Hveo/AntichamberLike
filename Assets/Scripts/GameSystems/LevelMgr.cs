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

public class LevelMgr : MonoBehaviour
{
    public static LevelMgr instance;
    public PlayerControl Player;
    public AudioClip LevelMusic;
    public Dictionary<string, int> States;
    public Dictionary<string, List<EventListener>> StatesSubscriber;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        States = new Dictionary<string, int>();
        StatesSubscriber = new Dictionary<string, List<EventListener>>();
        AudioMgr.PlayMusic(LevelMusic, true, true);
        GameUtilities.Init();
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
            if (States[StateID] != value)
            {
                States[StateID] = value;
                OnEventStateChange(StateID);
            }
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

    /// <summary>
    /// This Function will connect the event listener sent to the StateID. It creates a new entry if the state ID doesn't exist. You can send no listener if you just want the event to be created but not listened to
    /// </summary>
    /// <param name="StateID"></param>
    /// <param name="ev"></param>
    public void SubscribeToEvent(string StateID, EventListener ev)
    {
        if (!StatesSubscriber.ContainsKey(StateID))
        {
            if (ev != null)
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
        List<EventListener> Listeners = new List<EventListener>();

        if (!StatesSubscriber.TryGetValue(StateID, out Listeners))
            return;
        
        for (int i = 0; i < Listeners.Count; ++i)
        {
            Listeners[i].OnEventChange();
        }
    }

    private void OnDestroy()
    {
        AudioMgr.StopMusic();
    }
}

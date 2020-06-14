using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EAction
{
    TRIGGER_ENTER = (1 << 0),
    TRIGGER_EXIT = (1 << 1)
}

public enum EStateChange
{
    DEC,
    INC,
    SET
}

[System.Serializable]
public struct EventState
{
    public string StateID;
    public int Value;
    public EAction Action;
    public EStateChange StateValueModif;
}

public class TriggerStateModifier : MonoBehaviour
{
    public EventState[] EventStates;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < EventStates.Length; ++i)
        {
            EventState state = EventStates[i];
            if ((state.Action & EAction.TRIGGER_ENTER) != 0)
            {
                ApplyChanges(state.StateValueModif, state.StateID, state.Value);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < EventStates.Length; ++i)
        {
            EventState state = EventStates[i];
            if ((state.Action & EAction.TRIGGER_EXIT) != 0)
            {
                ApplyChanges(state.StateValueModif, state.StateID, state.Value);
            }
        }
    }

    void ApplyChanges(EStateChange ValueModification, string StateID, int value = 0)
    {
        int stateValue = GameMgr.instance.GetStateValue(StateID);

        if (stateValue == -99)
            return;

        switch (ValueModification)
        {
            case EStateChange.DEC: stateValue -= 1; break;
            case EStateChange.INC: stateValue += 1; break;
            case EStateChange.SET: stateValue = value; break;
            default: break;
        }

        GameMgr.instance.SetStateValue(StateID, stateValue);
    }
}

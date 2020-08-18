using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConditions : EventListener
{
    public EComparator Comparator;
    public int ValueToCompare;

    public void Start()
    {
        m_CurrentValue = 0;
        SubscribeToEvent();
        OnEventChange();
    }

    public override void OnEventChange()
    {
        m_CurrentValue = GameMgr.instance.GetStateValue(StateID);
        if (CheckCondition())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public bool CheckCondition()
    {
        switch (Comparator)
        {
            case EComparator.LESS: return m_CurrentValue < ValueToCompare;
            case EComparator.LEQUAL: return m_CurrentValue <= ValueToCompare;
            case EComparator.EQUAL: return m_CurrentValue == ValueToCompare;
            case EComparator.NOTEQUAL: return m_CurrentValue != ValueToCompare;
            case EComparator.GEQUAL: return m_CurrentValue >= ValueToCompare;
            case EComparator.GREATER: return m_CurrentValue > ValueToCompare;
            default: return false;
        }
    }
}

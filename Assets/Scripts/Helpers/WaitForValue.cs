using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForValue : CustomYieldInstruction
{
    private float m_Value;
    private float m_Threshold;
    EComparator m_Comparator;

    public override bool keepWaiting
    {
        get
        {
            switch (m_Comparator)
            {
                case EComparator.EQUAL: return m_Value != m_Threshold;
                case EComparator.GEQUAL: return m_Value < m_Threshold;
                case EComparator.GREATER: return m_Value <= m_Threshold;
                case EComparator.LEQUAL: return m_Value > m_Threshold;
                case EComparator.LESS: return m_Value >= m_Threshold;
                case EComparator.NOTEQUAL: return m_Value == m_Threshold;
                default: return false;
            }
        }
    }

    public WaitForValue(float val, float threshold, EComparator Comparator)
    {
        m_Value = val;
        m_Threshold = threshold;
        m_Comparator = Comparator;
    }
}

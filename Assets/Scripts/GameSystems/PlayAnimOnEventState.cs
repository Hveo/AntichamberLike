using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnEventState : EventListener
{
    public Animator[] Animators;
    public string[] StatesToSet;

    public override void OnEventChange()
    {
        m_CurrentValue = LevelMgr.instance.GetStateValue(StateID);

        if (StatesToSet.Length >= m_CurrentValue - 1 && Animators.Length >= m_CurrentValue - 1)
            Animators[m_CurrentValue - 1].SetBool(StatesToSet[m_CurrentValue - 1], true);
    }
}

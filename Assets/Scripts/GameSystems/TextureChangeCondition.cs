using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TextureState
{
    public int Value;
    public EComparator Comp;
    public Texture2D Tex;
}

public class TextureChangeCondition : EventListener
{
    public TextureState[] TextureStates;
    
    Renderer m_Rend;

    private void Start()
    {
        m_CurrentValue = 0;
        m_Rend = GetComponent<Renderer>();
        
        if (m_Rend != null)
            SubscribeToEvent();
    }

    public override void OnEventChange()
    {
        m_CurrentValue = GameMgr.instance.GetStateValue(StateID);
        for (int i = 0; i < TextureStates.Length; ++i)
        {
            if (CheckCondition(TextureStates[i]))
            {
                m_Rend.material.SetTexture("_MainTex", TextureStates[i].Tex);
                break;
            }
        }
    }

    public bool CheckCondition(TextureState TState)
    {
        switch (TState.Comp)
        {
            case EComparator.LESS: return m_CurrentValue < TState.Value;
            case EComparator.LEQUAL: return m_CurrentValue <= TState.Value;
            case EComparator.EQUAL: return m_CurrentValue == TState.Value;
            case EComparator.NOTEQUAL: return m_CurrentValue != TState.Value;
            case EComparator.GEQUAL: return m_CurrentValue >= TState.Value;
            case EComparator.GREATER: return m_CurrentValue > TState.Value;
            default: return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmBoxTimed : ConfirmBoxGeneric
{
    public TextMeshProUGUI Timer;
    float m_Timer;
    bool m_CountDown;

    public void StartTimer(float Timer)
    {
        m_CountDown = true;
        m_Timer = Timer;
    }

    private void LateUpdate()
    {
        if (!m_CountDown)
            return;

        m_Timer -= Time.deltaTime;
        Timer.text = Mathf.CeilToInt(m_Timer).ToString();

        if (m_Timer <= 0)
        {
            m_CountDown = false;
            Selectables[1].onClick.Invoke();
        }
    }

    new public void OnCancelInputPressed()
    {

    }

    private void OnDestroy()
    {
        m_CountDown = false;
    }
}

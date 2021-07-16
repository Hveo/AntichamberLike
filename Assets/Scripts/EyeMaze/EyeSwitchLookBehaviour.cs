using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSwitchLookBehaviour : MonoBehaviour
{
    Transform m_PlayerCamTransform;
    Material m_EyeMat;
    float m_Timer;
    string m_State;
    bool m_Switching;
    bool m_Interactible;

    // Start is called before the first frame update
    void Start()
    {
        if (m_EyeMat == null)
            m_EyeMat = GetComponent<Renderer>().material;

        m_Switching = false;
        m_State = "MazeWallState";
        m_PlayerCamTransform = Camera.main.transform;
        Reset();
        SetEyePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Timer <= 0.0f)
            return;

        if (!m_Interactible)
        {
            Reset();
            return;
        }

        RaycastHit hit = new RaycastHit();
        if (Vector3.Dot(m_PlayerCamTransform.forward, transform.up) < -0.98f && Physics.Raycast(transform.position, m_PlayerCamTransform.position - transform.position, out hit, Vector3.Distance(m_PlayerCamTransform.position, transform.position), Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_Timer -= Time.deltaTime;
            
            if (m_Timer <= 0.0f && m_EyeMat != null)
            {
                if (hit.collider.gameObject != LevelMgr.instance.Player.gameObject)
                    return;
            
                m_Switching = true;
                StartCoroutine(EyeSwitch());
            }            
        }
        else if (!m_Switching)
            Reset();
    }

    public void SetEyePosition()
    {
        StartCoroutine(EyeSwitch(false));
    }

    IEnumerator EyeSwitch(bool AlterState = true)
    {
        bool lookUp = LevelMgr.instance.GetStateValue(m_State) == 1;
        Vector4 offsetValue = m_EyeMat.GetVector("_PupilOffset");

        if (!lookUp)
        {
            while (offsetValue.y > -0.15f)
            {
                offsetValue.y = Mathf.MoveTowards(offsetValue.y, -0.15f, Time.deltaTime * 0.8f);
                m_EyeMat.SetVector("_PupilOffset", new Vector4(0.0f, offsetValue.y, 0.0f, 0.0f));
                yield return null;
            }

            if (AlterState)
                LevelMgr.instance.SetStateValue(m_State, 1);
        }
        else
        {
            while (offsetValue.y < 0.15f)
            {
                offsetValue.y = Mathf.MoveTowards(offsetValue.y, 0.15f, Time.deltaTime * 0.8f);
                m_EyeMat.SetVector("_PupilOffset", new Vector4(0.0f, offsetValue.y, 0.0f, 0.0f));
                yield return null;
            }

            if (AlterState)
                LevelMgr.instance.SetStateValue(m_State, 0);
        }

        yield return new WaitForSeconds(2.0f);
        Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_Interactible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        m_Interactible = false;
    }

    private void Reset()
    {
        m_Switching = false;
        m_Timer = 1.0f;
    }

    private void OnDisable()
    {
        Reset();
    }
}

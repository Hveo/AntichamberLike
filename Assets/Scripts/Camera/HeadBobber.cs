using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    public float BobbingSpeed;
    public float BobbingAmount;

    private CharacterController m_Player;
    private float m_DefaultPosY = 0;
    private float m_Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameMgr.instance.Player.Controller;
        m_DefaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(m_Player.velocity.x) > 0.1f || Mathf.Abs(m_Player.velocity.z) > 0.1f)
        {
            //Player is moving
            m_Timer += Time.deltaTime * BobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, m_DefaultPosY + Mathf.Sin(m_Timer) * BobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            m_Timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, m_DefaultPosY, Time.deltaTime * BobbingSpeed), transform.localPosition.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBox : IInteracitble
{
    public bool IsCarried { get; private set; }
    public Color HighlightColor;
    public Rigidbody Body
    {
        get { return m_Body; }
        private set { }
    }


    Renderer m_BoxRenderer;
    Color m_OriginalColor;
    Rigidbody m_Body;

    private void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_BoxRenderer = GetComponent<Renderer>();
        m_OriginalColor = m_BoxRenderer.material.GetColor("_Color");
    }

    public override void OnBeingInteractible()
    {
        m_BoxRenderer.material.SetColor("_Color", HighlightColor);
    }

    public override void OnStopBeingInteractible()
    {
        m_BoxRenderer.material.SetColor("_Color", m_OriginalColor);
    }

    public override void Interact()
    {
        if (IsCarried)
        {
            transform.parent = null;
            m_Body.isKinematic = false;
            IsCarried = false;
            KeepInteractability = false;
        }
        else
        {
            IsCarried = true;
            PlayerMove player = GameMgr.instance.Player;
            transform.parent = player.transform;
            transform.position = player.transform.position + player.Center + (player.transform.forward * 2.0f);
            m_Body.isKinematic = true;
            KeepInteractability = true;
        }
    }
}

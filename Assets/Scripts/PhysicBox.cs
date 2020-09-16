using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBox : IInteracitble
{
    public bool IsCarried { get; private set; }
    public Color HighlightColor;

    Renderer m_BoxRenderer;
    Color m_OriginalColor;
    Rigidbody m_body;

    private void Start()
    {
        m_body = GetComponent<Rigidbody>();
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
            m_body.isKinematic = false;
            IsCarried = false;
        }
        else
        {
            IsCarried = true;
            transform.parent = GameMgr.instance.Player.transform;
            m_body.isKinematic = true;
            //SetPositionInFrontOfPlayer;
        }
    }
}

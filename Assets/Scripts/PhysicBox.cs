using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
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
    BoxCollider m_Collider;

    private void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_BoxRenderer = GetComponent<Renderer>();
        m_Collider = GetComponent<BoxCollider>();
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
            GameMgr.instance.Player.ToggleCarryObject(gameObject, false);
            IsCarried = false;
            KeepInteractability = false;
            m_Body.isKinematic = false;
            m_Collider.enabled = true;
        }
        else
        {
            IsCarried = true;
            m_Body.isKinematic = true;
            m_Collider.enabled = false;
            GameMgr.instance.Player.ToggleCarryObject(gameObject, true);
            KeepInteractability = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class PhysicBox : IInteractible
{
    public bool IsCarried { get; private set; }
    public Color HighlightColor;
    public Rigidbody Body
    {
        get { return m_Body; }
        private set { }
    }


    private Renderer m_BoxRenderer;
    private Color m_OriginalColor;
    private Rigidbody m_Body;
    private BoxCollider m_Collider;
    private AudioSource m_AudioSrc;

    private void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_BoxRenderer = GetComponent<Renderer>();
        m_Collider = GetComponent<BoxCollider>();
        m_AudioSrc = GetComponent<AudioSource>();
        m_OriginalColor = m_BoxRenderer.material.GetColor("_Color");

        IsInteractible = true;
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
            LevelMgr.instance.Player.ToggleCarryObject(gameObject, false);
            IsCarried = false;
            KeepInteractability = false;
            m_Body.isKinematic = false;
            m_Collider.enabled = true;
            GameUtilities.HideBoxHelper();
        }
        else
        {
            IsCarried = true;
            m_Body.isKinematic = true;
            m_Collider.enabled = false;
            LevelMgr.instance.Player.ToggleCarryObject(gameObject, true);
            KeepInteractability = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        collisionForce = Mathf.Min(collisionForce, 1000.0f);

        if (!IsCarried)
        {
            m_AudioSrc.volume = (collisionForce - 100.0f) / (1000.0f - 100.0f);
            m_AudioSrc.Play();
        }
    }
}

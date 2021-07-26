using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInteractible : MonoBehaviour
{
    public bool KeepInteractability
    {
        get;
        protected set;
    }

    public bool IsInteractible
    {
        get;
        protected set;
    }

    public virtual void Interact() { }
    public virtual void OnBeingInteractible() { }

    public virtual void OnStopBeingInteractible() { }

    public virtual void SetInteractibilityState(bool value) 
    {
        IsInteractible = value;
    }
}

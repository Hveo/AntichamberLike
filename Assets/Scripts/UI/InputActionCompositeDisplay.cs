using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionCompositeDisplay : MonoBehaviour
{
    public string ActionName;
    // Start is called before the first frame update
    void Start()
    {
        InputHandler.onInputDeviceChangedDelegate += HotSwapComposite;
    }

    void HotSwapComposite()
    {
        Core.instance.StartCoroutine(SwapActionComposite());
    }

    IEnumerator SwapActionComposite()
    {
        InterfaceUtilities.Clear();
        yield return null;
        InterfaceUtilities.DisplayAction(ActionName, true);
    }

    private void OnDestroy()
    {
        InputHandler.onInputDeviceChangedDelegate -= HotSwapComposite;   
    }
}


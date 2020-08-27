using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetValue : MonoBehaviour
{
    public enum AnimationParameter
    {
        INT,
        BOOL,
        FLOAT,
        TRIGGER,
    }

    public Animator AnimatorObj;
    public AnimationParameter ParamType;
    public string ParameterName;
    public int intParam;
    public bool boolParam;
    public float floatParam;

    bool HasParameter()
    {
        for (int i = 0; i < AnimatorObj.parameterCount; ++i)
        {
            if (string.Compare(AnimatorObj.parameters[i].name, ParameterName) == 0)
                return true;
        }

        return false;
    }

    public void SetValue()
    {
        if (AnimatorObj == null || !HasParameter())
            return;

        switch (ParamType)
        {
            case AnimationParameter.INT:
            {
                AnimatorObj.SetInteger(ParameterName, intParam);
                break; 
            }
            case AnimationParameter.BOOL:
            {
                AnimatorObj.SetBool(ParameterName, boolParam);
                break;
            }
            case AnimationParameter.FLOAT:
            {
                AnimatorObj.SetFloat(ParameterName, floatParam);
                break; 
            }
            case AnimationParameter.TRIGGER:
            {
                AnimatorObj.SetTrigger(ParameterName);
                break;
            }
            default: break;
        }
    }
}

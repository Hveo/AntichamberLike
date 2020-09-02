using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimatorSetValue))]
public class AnimatorSetValue_CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        AnimatorSetValue inspect = (AnimatorSetValue)target;

        if (inspect == null)
            return;

        GUILayout.Label("Animator");
        inspect.AnimatorObj = EditorGUILayout.ObjectField(inspect.AnimatorObj, typeof(Animator), true, new GUILayoutOption[] { }) as Animator;
        inspect.ParamType = (AnimatorSetValue.AnimationParameter)EditorGUILayout.EnumPopup("Parameter Type", inspect.ParamType, new GUILayoutOption[] { });
        inspect.ParameterName = EditorGUILayout.TextField("Parameter Name", inspect.ParameterName, new GUILayoutOption[] { });

        GUILayout.BeginHorizontal();
        GUILayout.Label("Value");

        switch (inspect.ParamType)
        {
            case AnimatorSetValue.AnimationParameter.INT:
            {
                inspect.boolParam = false;
                inspect.floatParam = 0.0f;
                inspect.intParam = EditorGUILayout.IntField(inspect.intParam, new GUILayoutOption[] { });
                break;
            }
            case AnimatorSetValue.AnimationParameter.BOOL:
            {
                inspect.intParam = 0;
                inspect.floatParam = 0.0f;
                inspect.boolParam = EditorGUILayout.Toggle(inspect.boolParam, new GUILayoutOption[] { });
                break;
            }
            case AnimatorSetValue.AnimationParameter.FLOAT:
            {
                inspect.intParam = 0;
                inspect.boolParam = false;
                inspect.floatParam = EditorGUILayout.FloatField(inspect.floatParam, new GUILayoutOption[] { });
                break;
            }
            case AnimatorSetValue.AnimationParameter.TRIGGER:
            {
                GUILayout.Label("The trigger " + inspect.ParameterName + " will be toggled");
                break;
            }
            default: break;
        }

        GUILayout.EndHorizontal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtButtonPuzzle : MonoBehaviour
{
    public EyeLookAtPosition[] Eyes;
    public GameObject[] Buttons;
    public Renderer[] Lights;
    public AudioClip LightClip;

    int[] m_Combination;
    int m_CombinationIndex;
    bool m_Generating;

    void OnEnable()
    {
        SetupNewCombination();
    }

    void SetupNewCombination()
    {
        m_Generating = true;
        m_CombinationIndex = 0;

        for (int i = 0; i < Lights.Length; ++i)
        {
            Lights[i].material.SetColor("_Color", Color.red);
            Lights[i].material.SetColor("_EmissionColor", Color.red);
        }

        StartCoroutine(GenerateNewCombination());
    }

    IEnumerator GenerateNewCombination()
    {
        m_Combination = new int[5];
        int indexToSet = 0;

        while (true)
        {
            int index = Random.Range(0, Buttons.Length);

            if (indexToSet == 0 || index != m_Combination[indexToSet - 1])
            {
                m_Combination[indexToSet] = index;
                indexToSet++;

                if (indexToSet >= m_Combination.Length)
                {
                    LookAt(m_Combination[0]);
                    m_Generating = false;
                    yield break;
                }
            }

            yield return null;
        }
    }

    public void SubmitButton(int buttonIndex)
    {
        if (m_Generating || m_CombinationIndex >= m_Combination.Length)
            return;

        if (m_Combination[m_CombinationIndex] == buttonIndex)
        {
            Lights[m_CombinationIndex].material.SetColor("_Color", Color.green);
            Lights[m_CombinationIndex].material.SetColor("_EmissionColor", Color.green);

            m_CombinationIndex++;

            if (m_CombinationIndex >= m_Combination.Length)
            {
                if (Core.instance.BuiltInResources != null)
                    AudioMgr.PlaySound(Core.instance.BuiltInResources.SolvedPuzzleClip);

                LevelMgr.instance.SetStateValue("DisableScreen", 1);
                LookAt();
            }
            else
            {
                AudioMgr.PlaySound(LightClip);
                LookAt(m_Combination[m_CombinationIndex]);
            }
        }
        else if (Core.instance.BuiltInResources != null)
        {
            AudioMgr.PlaySound(Core.instance.BuiltInResources.WrongSolutionClip);
            SetupNewCombination();
        }
    }

    public void LookAt(int index = -1)
    {
        for (int i = 0; i < Eyes.Length; ++i)
        {
            if (index == -1)
                Eyes[i].LookAtObject(LevelMgr.instance.Player.gameObject);
            else
                Eyes[i].LookAtObject(Buttons[index]);
        }
    }
}

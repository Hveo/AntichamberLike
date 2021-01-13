using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilConsole : MonoBehaviour
{
    public Door DoorToOpen;

    public MeshFilter[] Shape;
    public MeshFilter[] DisplayedObjects;

    int[] m_CurrentIndexes;
    int m_UsedIndex;
    bool m_Solved;
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentIndexes = new int[4] { 0, 0, 0, 0 };

        if (Shape == null || DisplayedObjects == null)
            return;

        for (int i = 0; i < DisplayedObjects.Length; ++i)
        {
            DisplayedObjects[i].mesh = Shape[0].mesh;
        }
    }

    public void SubmitAnswer()
    {
        if (m_Solved)
            return;

        m_Solved = true;

        for (int i = 0; i < m_CurrentIndexes.Length; ++i)
        {
            if (m_CurrentIndexes[i] != i)
            {
                m_Solved = false;
                break;
            }
        }

        if (m_Solved)
        {
            if (Core.instance.BuiltInResources != null)
                AudioMgr.PlaySound(Core.instance.BuiltInResources.SolvedPuzzleClip);

            StartCoroutine(OpenDoor());
        }
        else if (Core.instance.BuiltInResources != null)
            AudioMgr.PlaySound(Core.instance.BuiltInResources.WrongSolutionClip);
    }

    public void SendManipulatedIndex(int Index)
    {
        m_UsedIndex = Index;
    }

    public void SwitchShape(bool Next)
    {
        if (!Next)
        {
            m_CurrentIndexes[m_UsedIndex] -= 1;

            if (m_CurrentIndexes[m_UsedIndex] < 0)
                m_CurrentIndexes[m_UsedIndex] = Shape.Length - 1;
        }
        else
        {
            m_CurrentIndexes[m_UsedIndex] += 1;

            if (m_CurrentIndexes[m_UsedIndex] >= Shape.Length)
                m_CurrentIndexes[m_UsedIndex] = 0;
        }

        DisplayedObjects[m_UsedIndex].mesh = Shape[m_CurrentIndexes[m_UsedIndex]].mesh;
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2.0f);
        DoorToOpen.Unlock();
        yield return new WaitForSeconds(1.0f);
        DoorToOpen.Open();
    }
}

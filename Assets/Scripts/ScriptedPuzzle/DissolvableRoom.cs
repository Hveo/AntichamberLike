using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightColor : ushort
{
    RED = 0,
    BLUE = 1,
    YELLOW = 2,
    GREEN = 3,
}

public class DissolvableRoom : MonoBehaviour
{
    public DissolvableRoom[] OtherRoomsToRotate;
    public GameObject[] Walls;
    public GameObject[] NeighbourRooms;
    public Light RoomLight;
    public LightColor StartColor;
    public ushort m_ColorIndex
    {
        get;
        private set;
    }

    List<Material> m_Mat = new List<Material>();
    bool m_RunningCrt = false;

    private void Start()
    {
        for (int i = 0; i < Walls.Length; ++i)
        {
            if (Walls[i] != null)
                m_Mat.Add(Walls[i].GetComponent<Renderer>().material);
        }

        ResetState();
    }
    
    List<Renderer> GatherNeighbours()
    {
        List<Renderer> neighbours = new List<Renderer>();

        for (int i = 0; i < Walls.Length; ++i)
        {
            for (int j = 0; j < NeighbourRooms.Length; ++j)
            {
                if (GameUtilities.IsPositionBetweenAB(transform.position, NeighbourRooms[j].transform.position, Walls[i].transform.position))
                {
                    neighbours.Add(Walls[i].GetComponent<Renderer>());
                    break;
                }
            }
        }

        return neighbours;
    }

    public void RotateRoom()
    {
        if (!m_RunningCrt)
            StartCoroutine(LockAndRotate(false, false));
    }

    IEnumerator LockAndRotate(bool Reset = false, bool RotationInitiator = false)
    {
        m_RunningCrt = true;
        Color[] ColorOrder = new Color[] { Color.red, Color.blue, Color.yellow, Color.green };
        Color color = ColorOrder[0];

        if (RotationInitiator)
        {
            for (int i = 0; i < OtherRoomsToRotate.Length; ++i)
                OtherRoomsToRotate[i].RotateRoom();
        }

        if (Reset)
        {
            m_ColorIndex = (ushort)StartColor;
            color = ColorOrder[m_ColorIndex];
        }
        else
        {
            for (int i = 0; i < ColorOrder.Length; ++i)
            {
                if (m_ColorIndex == i)
                {
                    if (i == ColorOrder.Length - 1)
                    {
                        m_ColorIndex = 0;
                        color = ColorOrder[m_ColorIndex];
                    }
                    else
                    {
                        m_ColorIndex++;
                        color = ColorOrder[m_ColorIndex];
                    }

                    break;
                }
            }
        }

        float Amount = 1.0f;

        while (Amount > 0.0)
        {
            Amount = Mathf.MoveTowards(Amount, 0.0f, Time.deltaTime * 0.75f);

            for (int i = 0; i < m_Mat.Count; ++i)
            {
                Walls[i].GetComponent<BoxCollider>().enabled = true;

                if (m_Mat[i].GetFloat("_Amount") == 0.0f)
                    continue;

                m_Mat[i].SetFloat("_Amount", Amount);
            }

            yield return null;
        }

        Quaternion DesiredRot = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, 90.0f, 0.0f));

        while (Mathf.Abs(transform.rotation.eulerAngles.y - DesiredRot.eulerAngles.y) > 0.03f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRot, Time.deltaTime * 2.5f);
            RoomLight.color = Color.Lerp(RoomLight.color, color, Time.deltaTime * 2.5f);
            yield return null;
        }

        transform.rotation = DesiredRot;

        List<Renderer> wallsToOpen = GatherNeighbours();

        while (Amount < 1.0)
        {
            Amount = Mathf.MoveTowards(Amount, 1.0f, Time.deltaTime * 0.75f);

            for (int i = 0; i < wallsToOpen.Count; ++i)
            {
                wallsToOpen[i].material.SetFloat("_Amount", Amount);
            }

            yield return null;
        }

        for (int i = 0; i < wallsToOpen.Count; ++i)
            wallsToOpen[i].GetComponent<BoxCollider>().enabled = false;

        LightMazeExit.instance.OnRoomsRotated();
        m_RunningCrt = false;
    }

    public void ResetState()
    {
        if (!m_RunningCrt)
            StartCoroutine(LockAndRotate(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_RunningCrt)
            StartCoroutine(LockAndRotate(false, true));
    }
}

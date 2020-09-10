using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LightMazeExit : MonoBehaviour
{
    public static LightMazeExit instance;

    public DissolvableRoom[] Rooms;
    public int ExitRoomIndex;
    public Transform Exit;

    ushort m_EventCallCount;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        m_EventCallCount = 0;
    }

    public void OnRoomsRotated()
    {
        m_EventCallCount++;

        if (m_EventCallCount < Rooms.Length - 1) //We're rotating only three of them
            return;

        for (int i = 0; i < Rooms.Length; ++i)
        {
            if (Rooms[i].m_ColorIndex != 3)
                return;
        }

        for (int i = 0; i < Rooms[ExitRoomIndex].Walls.Length; ++i)
        {
            GameObject Wall = Rooms[ExitRoomIndex].Walls[i];
            Vector3 bias = Rooms[ExitRoomIndex].transform.position + (Exit.position - Rooms[ExitRoomIndex].transform.position).normalized;

            if (GameUtilities.IsPositionBetweenAB(bias, Exit.position, Wall.transform.position))
            {
                Wall.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(GameUtilities.DissolveMesh(Wall.GetComponent<Renderer>(), true, 0.75f));
                break;
            }

        }

        m_EventCallCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < Rooms.Length; ++i)
        {
            Rooms[i].GetComponent<BoxCollider>().enabled = false;
        }
    }
}

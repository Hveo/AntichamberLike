using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    public GameObject[] ToEnable;
    public AudioClip WallDisableSound;
    private void Start()
    {
        SetRoomActive(false);
    }

    public void SetRoomActive(bool active)
    {
        for (int i = 0; i < ToEnable.Length; ++i)
        {
            ToEnable[i].SetActive(active);
        }
    }

    public void BreakWall(GameObject obj)
    {
        AudioMgr.PlaySound(WallDisableSound);
        StartCoroutine(GameUtilities.DissolveMesh(obj.GetComponent<Renderer>(), true, 0.5f));
        Collider coll = obj.GetComponent<Collider>();

        if (coll != null)
            coll.enabled = false;
    }

    public void ForceEyeState(int value)
    {
        LevelMgr.instance.SetStateValue("MazeWallState", value);
    }

    private void OnTriggerEnter(Collider other)
    {
        SetRoomActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetRoomActive(false);
    }
}

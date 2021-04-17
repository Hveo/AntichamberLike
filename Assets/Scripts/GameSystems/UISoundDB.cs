using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISoundDB", menuName = "Overthink/UISoundDB")]
public class UISoundDB : ScriptableObject
{
    [System.Serializable]
    public struct UISound
    {
        public string SoundID;
        public AudioClip Clip;
    }

    public List<UISound> SoundDB;

    public AudioClip GetClip(string ID)
    {
        if (string.IsNullOrEmpty(ID))
            return null;

        for (int i = 0; i < SoundDB.Count; ++i)
        {
            if (string.CompareOrdinal(SoundDB[i].SoundID, ID) == 0)
                return SoundDB[i].Clip;
        }

        return null;
    }
}

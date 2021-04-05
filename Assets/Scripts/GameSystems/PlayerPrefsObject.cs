using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefs", menuName = "Overthink/PlayerPrefs"), System.Serializable]
public class PlayerPrefsObject : ScriptableObject
{
    public float MouseSensitivity;
    public float StickSensitivity;
    public bool InvertXAxis;
    public bool InvertYAxis;
    public float MusicVolume;
    public float FXVolume;
    public Language CurrentLanguage;
}

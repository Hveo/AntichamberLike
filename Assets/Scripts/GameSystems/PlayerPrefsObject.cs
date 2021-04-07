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

    public PlayerPrefsObject GetDeepCopy()
    {
        PlayerPrefsObject clone = new PlayerPrefsObject();

        clone.MouseSensitivity = this.MouseSensitivity;
        clone.StickSensitivity = this.StickSensitivity;
        clone.InvertXAxis = this.InvertXAxis;
        clone.InvertYAxis = this.InvertYAxis;
        clone.MusicVolume = this.MusicVolume;
        clone.FXVolume = this.FXVolume;
        clone.CurrentLanguage = this.CurrentLanguage;

        return clone;
    }
}

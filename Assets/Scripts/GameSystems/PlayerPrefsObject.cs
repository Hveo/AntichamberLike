using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerPrefs", menuName = "Overthink/PlayerPrefs"), System.Serializable]
public class PlayerPrefsObject : ScriptableObject
{
    public string Resolution;
    public string FrameLimit;
    public float MouseSensitivity;
    public float StickSensitivity;
    public bool InvertXAxis;
    public bool InvertYAxis;
    public float MusicVolume;
    public float FXVolume;
    public float UIVolume;
    public Language CurrentLanguage;

    public PlayerPrefsObject GetDeepCopy()
    {
        PlayerPrefsObject clone = new PlayerPrefsObject();

        clone.Resolution = this.Resolution;
        clone.FrameLimit = this.FrameLimit;
        clone.MouseSensitivity = this.MouseSensitivity;
        clone.StickSensitivity = this.StickSensitivity;
        clone.InvertXAxis = this.InvertXAxis;
        clone.InvertYAxis = this.InvertYAxis;
        clone.MusicVolume = this.MusicVolume;
        clone.FXVolume = this.FXVolume;
        clone.UIVolume = this.UIVolume;
        clone.CurrentLanguage = this.CurrentLanguage;

        return clone;
    }

    public void SetDefaultValue()
    {
        this.Resolution = "1920x1080";
        this.FrameLimit = "60";
        this.MouseSensitivity = 10.0f;
        this.StickSensitivity = 150.0f;
        this.InvertXAxis = false;
        this.InvertYAxis = false;
        this.MusicVolume = 100.0f;
        this.FXVolume = 100.0f;
        this.UIVolume = 70.0f;
        this.CurrentLanguage = Language.EN;
    }

    public int[] ParseResolution()
    {
        int[] resolution = new int[2];
        int index = this.Resolution.IndexOf("x");
        resolution[0] = int.Parse(this.Resolution.Substring(0, index));
        resolution[1] = int.Parse(this.Resolution.Substring(index + 1));

        return resolution;
    }
}

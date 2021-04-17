using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "BuiltInGameResources", menuName = "Overthink/BuiltInGameResources")]
public class BuiltInGameResources : ScriptableObject
{
    public GameObject PlayerObj;
    public AudioMixer Mixer;
    public AudioClip SolvedPuzzleClip;
    public AudioClip WrongSolutionClip;
    public AudioClip LevelMusic;
    public UISoundDB SoundDB;
}

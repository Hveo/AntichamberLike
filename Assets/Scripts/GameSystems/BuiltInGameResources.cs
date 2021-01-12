using UnityEngine;

[CreateAssetMenu(fileName = "BuiltInGameResources", menuName = "Overthink/BuiltInGameResources")]
public class BuiltInGameResources : ScriptableObject
{
    public GameObject PlayerObj;
    public AudioClip SolvedPuzzleClip;
    public AudioClip WrongSolutionClip;
    public AudioClip LevelMusic;
}

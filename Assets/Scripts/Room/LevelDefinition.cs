using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Definition")]
public class LevelDefinition : ScriptableObject
{
    public int levelIndex = 1;
    public GameObject[] rooms; // size 10
}
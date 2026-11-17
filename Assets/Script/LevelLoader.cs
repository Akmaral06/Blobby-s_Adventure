using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public LevelData currentLevel;

    void Start()
    {
        foreach (var spawn in currentLevel.enemies)
        {
            Instantiate(spawn.enemyPrefab, spawn.position, Quaternion.identity);
        }
    }
}
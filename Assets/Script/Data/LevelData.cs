using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game Data/Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemyPrefab;
        public Vector3 position;
    }

    public List<EnemySpawn> enemies;
    public float timeLimit = 90f;
    public int goalAmount = 3; 
}
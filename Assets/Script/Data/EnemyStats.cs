using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Game Data/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Patrol")]
    public float patrolSpeed = 1.5f;
    public float patrolDistance = 2f;   

    [Header("Vision")]
    public float visionRange = 6f;
    public float visionAngle = 100f;

    [Header("Chase & Attack")]
    public float chaseSpeed = 3f;
    public float attackRange = 1.2f;
    public float windUpTime = 0.4f;
    public float lungeSpeed = 6f;
    public float lungeDuration = 0.25f;
    public float cooldown = 1.2f;

    [Header("Health & Damage")]
    public float maxHP = 20f;
    public float damage = 10f;
}
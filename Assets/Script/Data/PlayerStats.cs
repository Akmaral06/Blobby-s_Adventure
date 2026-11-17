using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game Data/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Health")]
    public int maxHp = 3;

    [Header("Invincibility after hit")]
    public int invincibilityFlashes = 10;
    public float invincibilityFlashInterval = 0.2f;
}
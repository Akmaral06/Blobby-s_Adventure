using UnityEngine;

[CreateAssetMenu(fileName = "NewShooterStats", menuName = "Game Data/Shooter Stats")]
public class ShooterStats : ScriptableObject
{
    [Header("Vision")]
    public float fireRange = 7f;
    public float visionAngle = 100f;

    [Header("Attack")]
    public float windUpTime = 0.6f;
    public float cooldown = 1.6f;
    public GameObject bulletPrefab;
    public Vector2 muzzleOffset = new Vector2(0.6f, 0.3f);
    public AudioClip shootClip;
}
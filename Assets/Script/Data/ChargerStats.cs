using UnityEngine;

[CreateAssetMenu(fileName = "NewChargerStats", menuName = "Game Data/Charger Stats")]
public class ChargerStats : ScriptableObject
{
    [Header("Vision")]
    public float alertRange = 5f;
    public float visionAngle = 100f;

    [Header("Attack")]
    public float windUpTime = 0.7f;
    public float chargeSpeed = 9f;
    public float chargeDuration = 0.5f;
    public float recoverTime = 1f;
}
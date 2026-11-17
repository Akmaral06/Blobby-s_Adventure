using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public enum State { Idle, Aim, Fire, Cooldown }

    [Header("Balance data")]
    public ShooterStats stats;

    [Header("Vision (layer references, not balance numbers)")]
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    private State state = State.Idle;
    private SpriteRenderer sr;
    private AudioSource src;
    private Transform player;
    private float stateTimer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.Aim: AimState(); break;
            case State.Fire: FireState(); break;
            case State.Cooldown: CooldownTick(); break;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        if (toPlayer.magnitude > stats.fireRange) return false;

        Vector2 facing = sr.flipX ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(facing, toPlayer.normalized);
        if (angle > stats.visionAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    void IdleState()
    {
        if (CanSeePlayer())
        {
            state = State.Aim;
            stateTimer = 0f;
        }
    }

    void AimState()
    {
        if (!CanSeePlayer())
        {
            state = State.Idle;
            sr.color = Color.white;
            return;
        }

        stateTimer += Time.deltaTime;
        sr.color = Color.Lerp(Color.white, Color.red, stateTimer / stats.windUpTime);

        if (stateTimer >= stats.windUpTime)
            state = State.Fire;
    }

    void FireState()
    {
        sr.color = Color.white;

        float dir = sr.flipX ? 1f : -1f;
        Vector3 pos = transform.position + new Vector3(dir * stats.muzzleOffset.x, stats.muzzleOffset.y, 0f);
        GameObject bullet = Instantiate(stats.bulletPrefab, pos, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().direction = dir;

        if (src != null && stats.shootClip != null)
            src.PlayOneShot(stats.shootClip);

        state = State.Cooldown;
        stateTimer = 0f;
    }

    void CooldownTick()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= stats.cooldown)
            state = State.Idle;
    }
}
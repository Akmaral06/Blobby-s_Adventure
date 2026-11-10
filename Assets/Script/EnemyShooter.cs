using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public enum State { Idle, Aim, Fire, Cooldown }

    [Header("Vision")]
    public float fireRange = 7f;
    public float visionAngle = 100f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Attack")]
    public float windUpTime = 0.6f;
    public float cooldown = 1.6f;
    public GameObject bulletPrefab;
    public Vector2 muzzleOffset = new Vector2(0.6f, 0.3f);
    public AudioClip shootClip;

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
    if (toPlayer.magnitude > fireRange) return false;

    Vector2 facing = sr.flipX ? Vector2.right : Vector2.left;
    float angle = Vector2.Angle(facing, toPlayer.normalized);
    if (angle > visionAngle / 2f) return false;

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
        sr.color = Color.Lerp(Color.white, Color.red, stateTimer / windUpTime);   

        if (stateTimer >= windUpTime)
            state = State.Fire;
    }

    void FireState()
    {
        sr.color = Color.white;

        float dir = sr.flipX ? 1f : -1f;
        Vector3 pos = transform.position + new Vector3(dir * muzzleOffset.x, muzzleOffset.y, 0f);
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().direction = dir;

        if (src != null && shootClip != null)
            src.PlayOneShot(shootClip);

        state = State.Cooldown;
        stateTimer = 0f;
    }

    void CooldownTick()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= cooldown)
            state = State.Idle;
    }
}
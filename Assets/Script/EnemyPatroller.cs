using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Cooldown }

    [Header("Balance data")]
    public EnemyStats stats;

    [Header("Vision (layer references, not balance numbers)")]
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Sound")]
    public AudioClip windUpClip;

    private State state = State.Patrol;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private AudioSource src;
    private float stateTimer;
    private float lastX;

    private float startX;
    private float patrolDir = -1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    void Start()
    {
        lastX = rb.position.x;
        startX = rb.position.x;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
            case State.Cooldown: CooldownTick(); break;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        if (toPlayer.magnitude > stats.visionRange) return false;

        Vector2 facing = sr.flipX ? Vector2.left : Vector2.right;
        float angle = Vector2.Angle(facing, toPlayer.normalized);
        if (angle > stats.visionAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    void Patrol()
    {
        float leftBound = startX - stats.patrolDistance;
        float rightBound = startX + stats.patrolDistance;

        Vector2 next = rb.position + new Vector2(patrolDir * stats.patrolSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(next);
        FaceMoveDirection(next.x);

        if (next.x <= leftBound) patrolDir = 1f;
        else if (next.x >= rightBound) patrolDir = -1f;

        if (CanSeePlayer())
            state = State.Chase;
    }

    void Chase()
    {
        if (!CanSeePlayer())
        {
            state = State.Patrol;
            return;
        }

        float dist = Vector2.Distance(rb.position, player.position);
        if (dist <= stats.attackRange)
        {
            state = State.Attack;
            stateTimer = 0f;
            sr.flipX = player.position.x < rb.position.x;
            sr.color = Color.yellow;
            SFX.Play(src, windUpClip);
            return;
        }

        float dir = player.position.x > rb.position.x ? 1f : -1f;
        Vector2 next = rb.position + new Vector2(dir * stats.chaseSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(next);
        FaceMoveDirection(next.x);
    }

    void Attack()
    {
        stateTimer += Time.fixedDeltaTime;

        if (stateTimer < stats.windUpTime)
            return;

        if (stateTimer < stats.windUpTime + stats.lungeDuration)
        {
            sr.color = Color.white;
            float dir = sr.flipX ? -1f : 1f;
            rb.MovePosition(rb.position + new Vector2(dir * stats.lungeSpeed * Time.fixedDeltaTime, 0f));
            return;
        }

        state = State.Cooldown;
        stateTimer = 0f;
    }

    void CooldownTick()
    {
        stateTimer += Time.fixedDeltaTime;
        if (stateTimer >= stats.cooldown)
            state = CanSeePlayer() ? State.Chase : State.Patrol;
    }

    void FaceMoveDirection(float newX)
    {
        if (newX > lastX) sr.flipX = false;
        else if (newX < lastX) sr.flipX = true;
        lastX = newX;
    }
}
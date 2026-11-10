using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Cooldown }

    [Header("Patrol (drag empty GameObjects here as waypoints)")]
    public Transform[] waypoints;
    public float patrolSpeed = 1.5f;
    public float waypointThreshold = 0.15f;

    [Header("Vision")]
    public float visionRange = 6f;
    public float visionAngle = 100f;
    public LayerMask obstacleMask;   
    public LayerMask playerMask;     

    [Header("Attack")]
    public float chaseSpeed = 3f;
    public float attackRange = 1.2f;
    public float windUpTime = 0.4f;
    public float lungeSpeed = 6f;
    public float lungeDuration = 0.25f;
    public float cooldown = 1.2f;

    private State state = State.Patrol;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private int waypointIndex;
    private float stateTimer;
    private float lastX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        lastX = rb.position.x;
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
    if (toPlayer.magnitude > visionRange) return false;

    Vector2 facing = sr.flipX ? Vector2.left : Vector2.right;
    float angle = Vector2.Angle(facing, toPlayer.normalized);
    if (angle > visionAngle / 2f) return false;

    RaycastHit2D hit = Physics2D.Raycast(rb.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask | playerMask);
    return hit.collider != null && hit.collider.CompareTag("Player");
}

    void Patrol()
    {
        if (waypoints.Length > 0)
        {
            Transform target = waypoints[waypointIndex];
            Vector2 next = Vector2.MoveTowards(rb.position, target.position, patrolSpeed * Time.fixedDeltaTime);
            rb.MovePosition(next);
            FaceMoveDirection(next.x);

            if (Vector2.Distance(rb.position, target.position) < waypointThreshold)
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }

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
        if (dist <= attackRange)
        {
            state = State.Attack;
            stateTimer = 0f;
            sr.flipX = player.position.x < rb.position.x;
            sr.color = Color.yellow;          
            return;
        }

        float dir = player.position.x > rb.position.x ? 1f : -1f;
        Vector2 next = rb.position + new Vector2(dir * chaseSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(next);
        FaceMoveDirection(next.x);
    }

    void Attack()
    {
        stateTimer += Time.fixedDeltaTime;

        if (stateTimer < windUpTime)
            return;                            

        if (stateTimer < windUpTime + lungeDuration)
        {
            sr.color = Color.white;
            float dir = sr.flipX ? -1f : 1f;
            rb.MovePosition(rb.position + new Vector2(dir * lungeSpeed * Time.fixedDeltaTime, 0f));
            return;                            
        }

        state = State.Cooldown;
        stateTimer = 0f;
    }

    void CooldownTick()
    {
        stateTimer += Time.fixedDeltaTime;
        if (stateTimer >= cooldown)
            state = CanSeePlayer() ? State.Chase : State.Patrol;
    }

    void FaceMoveDirection(float newX)
    {
        if (newX > lastX) sr.flipX = false;
        else if (newX < lastX) sr.flipX = true;
        lastX = newX;
    }
}
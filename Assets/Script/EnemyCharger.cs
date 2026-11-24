using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    public enum State { Idle, WindUp, Charge, Recover }

    [Header("Balance data")]
    public ChargerStats stats;

    [Header("Vision (layer references, not balance numbers)")]
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Sound")]
    public AudioClip windUpClip;

    private State state = State.Idle;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource src;
    private Transform player;
    private float stateTimer;
    private Vector2 chargeDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle: IdleState(); break;
            case State.WindUp: WindUpState(); break;
            case State.Charge: ChargeState(); break;
            case State.Recover: RecoverState(); break;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        if (toPlayer.magnitude > stats.alertRange) return false;

        Vector2 facing = sr.flipX ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(facing, toPlayer.normalized);
        if (angle > stats.visionAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    void IdleState()
    {
        rb.linearVelocity = Vector2.zero;

        if (CanSeePlayer())
        {
            state = State.WindUp;
            stateTimer = 0f;
            chargeDir = (player.position.x > rb.position.x) ? Vector2.right : Vector2.left;
            sr.flipX = chargeDir.x < 0f;
            SFX.Play(src, windUpClip);
        }
    }

    void WindUpState()
    {
        stateTimer += Time.fixedDeltaTime;
        sr.color = Color.Lerp(Color.white, Color.red, stateTimer / stats.windUpTime);

        if (stateTimer >= stats.windUpTime)
        {
            state = State.Charge;
            stateTimer = 0f;
        }
    }

    void ChargeState()
    {
        sr.color = Color.white;
        rb.linearVelocity = chargeDir * stats.chargeSpeed;

        stateTimer += Time.fixedDeltaTime;
        if (stateTimer >= stats.chargeDuration)
        {
            state = State.Recover;
            stateTimer = 0f;
        }
    }

    void RecoverState()
    {
        rb.linearVelocity = Vector2.zero;
        stateTimer += Time.fixedDeltaTime;
        if (stateTimer >= stats.recoverTime)
            state = State.Idle;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (state == State.Charge && c.gameObject.CompareTag("Ground"))
        {
            state = State.Recover;
            stateTimer = 0f;
        }
    }
}
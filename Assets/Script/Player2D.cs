using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    [Header("Balance data")]
    public PlayerStats stats;

    [Header("Audio (not balance numbers)")]
    public AudioClip jumpClip;
    public AudioClip coinClip;
    public AudioClip loseClip;

    public int coins { get; private set; }

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Health health;
    private Shooter shooter;
    private AudioSource src;
    private Vector2 move;
    private bool onGround;
    private int groundContacts;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        shooter = GetComponent<Shooter>();
        src = GetComponent<AudioSource>();
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            onGround = false;
            SFX.Play(src, jumpClip);
        }
    }

    public void OnFire()
    {
        shooter.Shoot();
    }

    void Update()
    {
        anim.SetBool("Grounded", onGround);

        if (transform.position.y < -8f)
            RestartLevel();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move.x * stats.speed, rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(move.x));

        if (move.x > 0) sr.flipX = false;
        if (move.x < 0) sr.flipX = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            SFX.Play(src, coinClip);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            groundContacts++;
            onGround = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
            health.TakeDamage(1);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            groundContacts--;
            if (groundContacts <= 0)
            {
                groundContacts = 0;
                onGround = false;
            }
        }
    }

    public void RestartLevel()
    {
        SFX.Play(src, loseClip);
        Invoke(nameof(ReloadScene), 0.6f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
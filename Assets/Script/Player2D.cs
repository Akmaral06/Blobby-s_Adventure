using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public AudioClip jumpClip;       
    public AudioClip coinClip;       

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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            onGround = false;
            Play(jumpClip);
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
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
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
            Play(coinClip);
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

    void Play(AudioClip clip)
    {
        if (src != null && clip != null)
            src.PlayOneShot(clip);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
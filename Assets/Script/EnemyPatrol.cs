using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float distance = 3f;
    public float speed = 1.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 start;
    private float lastX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        start = rb.position;
        lastX = start.x;
    }

    void FixedUpdate()
    {
        float x = start.x + Mathf.PingPong(Time.time * speed, distance) - distance / 2f;
        rb.MovePosition(new Vector2(x, start.y));

        if (x > lastX) sr.flipX = false;
        else if (x < lastX) sr.flipX = true;
        lastX = x;
    }
}
using UnityEngine;

public class BossProjectile : Hazard
{
    public float lifeTime = 6f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector2 velocity, float gravity)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        rb.linearVelocity = velocity;
    }
}
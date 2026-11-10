using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float direction = 1f;
    public int damage = 1;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * speed, 0f);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
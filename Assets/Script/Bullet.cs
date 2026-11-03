using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float direction = 1f;
    public int damage = 1;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * speed, 0f);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        BossHealth boss = other.GetComponentInParent<BossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
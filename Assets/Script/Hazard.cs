using UnityEngine;

public class Hazard : MonoBehaviour
{
    public bool destroyOnGround = false;  

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Health health = c.gameObject.GetComponent<Health>();
            if (health != null)
                OnHitPlayer(health);

            Destroy(gameObject);
        }
        else if (destroyOnGround && c.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnHitPlayer(Health player)
    {
        player.TakeDamage(1);
    }
}
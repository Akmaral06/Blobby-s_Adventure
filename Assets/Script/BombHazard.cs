using UnityEngine;

public class BombHazard : Hazard
{
    public GameObject explosionPrefab;   

    void Start()
    {
        Invoke(nameof(Explode), 2f);
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            GameObject boom = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(boom, 0.6f);         
        }
        Destroy(gameObject);
    }

    protected override void OnHitPlayer(Health p)
    {
        p.TakeDamage(2);                 
    }
}
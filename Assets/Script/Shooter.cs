using UnityEngine;                       

public class Shooter : MonoBehaviour     
{
    public GameObject bulletPrefab;      
    public float cooldown = 0.3f;        
    public Vector2 offset = new Vector2(0.6f, 0.5f);  
    public AudioClip shootClip;          

    private float nextShot;              
    private SpriteRenderer sr;           
    private AudioSource src;            

    void Awake()                         
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    
    public void Shoot()
    {
        if (Time.time < nextShot) return;     

        nextShot = Time.time + cooldown;     

        float dir = sr.flipX ? -1f : 1f;      

        Vector3 pos = transform.position + new Vector3(dir * offset.x, offset.y, 0f);
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bullet.GetComponent<Bullet>().direction = dir;   

        if (src != null && shootClip != null)
            src.PlayOneShot(shootClip);    
    }
}
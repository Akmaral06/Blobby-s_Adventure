using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int hp = 3;
    public int maxHp = 3;
    public AudioClip hurtClip;      
    public AudioClip healClip;      

    private bool invincible;
    private SpriteRenderer sr;
    private AudioSource src;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;              

        hp -= dmg;
        Debug.Log("HP: " + hp);
        Play(hurtClip);

        if (hp <= 0)
        {
            GameOver();
            return;
        }

        StartCoroutine(Invincibility());    
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);  
        Debug.Log("HP: " + hp);
        Play(healClip);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)              
        {
            sr.enabled = !sr.enabled;             
            yield return new WaitForSeconds(0.2f); 
        }
        sr.enabled = true;
        invincible = false;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Play(AudioClip clip)
    {
        if (src != null && clip != null)
            src.PlayOneShot(clip);
    }
}
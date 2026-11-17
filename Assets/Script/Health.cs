using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Balance data")]
    public PlayerStats stats;

    [Header("Audio (not balance numbers)")]
    public AudioClip hurtClip;
    public AudioClip healClip;

    public int hp { get; private set; }

    private bool invincible;
    private SpriteRenderer sr;
    private AudioSource src;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    void Start()
    {
        hp = stats.maxHp;
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
        hp = Mathf.Min(hp + amount, stats.maxHp);
        Debug.Log("HP: " + hp);
        Play(healClip);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < stats.invincibilityFlashes; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(stats.invincibilityFlashInterval);
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
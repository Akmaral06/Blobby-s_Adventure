using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHp = 10;
    public int hp;
    public Slider hpBar;
    public AudioClip hitClip;
    public AudioClip winClip;
    public string nextScene = "Win";

    private SpriteRenderer sr;
    private AudioSource src;
    private bool dead;

    public bool Phase2 { get { return hp <= maxHp / 2; } }
    public bool IsDead { get { return dead; } }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    void Start()
    {
        hp = maxHp;
        if (hpBar != null)
        {
            hpBar.maxValue = maxHp;
            hpBar.value = hp;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;

        hp -= dmg;
        if (hpBar != null) hpBar.value = Mathf.Max(hp, 0);
        Play(hitClip);
        StartCoroutine(Flash());

        if (hp <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (!dead) sr.color = Color.white;
    }

    IEnumerator Die()
    {
        dead = true;
        sr.color = Color.gray;
        Play(winClip);

        foreach (Hazard h in FindObjectsByType<Hazard>(FindObjectsSortMode.None))
            Destroy(h.gameObject);

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextScene);
    }

    void Play(AudioClip clip)
    {
        if (src != null && clip != null)
            src.PlayOneShot(clip);
    }
}
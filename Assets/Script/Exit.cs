using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public string nextScene = "Level2";
    public int coinsNeeded = 3;
    public Animator gateAnimator;     
    public AudioClip winClip;         

    private Player2D player;
    private AudioSource src;
    private bool loading;

    void Start()
    {
        player = FindFirstObjectByType<Player2D>();
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gateAnimator != null && player != null)
            gateAnimator.SetBool("Open", player.coins >= coinsNeeded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (loading || !other.CompareTag("Player")) return;

        if (player.coins >= coinsNeeded)
            StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        loading = true;
        if (src != null && winClip != null)
            src.PlayOneShot(winClip);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextScene);
    }
}
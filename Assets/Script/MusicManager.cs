using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource sourceA;
    public AudioSource sourceB;

    public AudioClip menuMusic;
    public AudioClip levelMusic;

    public float crossfadeDuration = 1f;

    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private AudioClip currentClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        activeSource = sourceA;
        inactiveSource = sourceB;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name);
    }

    void PlayForScene(string sceneName)
    {
        AudioClip target = (sceneName == "MainMenu") ? menuMusic : levelMusic;
        if (target == currentClip) return;
        StartCoroutine(CrossfadeTo(target));
    }

    IEnumerator CrossfadeTo(AudioClip newClip)
    {
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();
        currentClip = newClip;

        float t = 0f;
        float startVolActive = activeSource.volume;

        while (t < crossfadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float ratio = t / crossfadeDuration;
            inactiveSource.volume = Mathf.Lerp(0f, 1f, ratio);
            activeSource.volume = Mathf.Lerp(startVolActive, 0f, ratio);
            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = 1f;

        AudioSource temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;
    }
}
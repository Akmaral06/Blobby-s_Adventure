using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject touchControls;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio Mixer")]
    public AudioMixer mixer;
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot pausedSnapshot;
    public float snapshotTransition = 0.3f;

    [Header("UI click sound")]
    public AudioSource uiSource;
    public AudioClip uiClickClip;

    private bool isPaused;

    void Start()
    {
        SaveData data = SaveManager.Load();

        if (musicSlider != null)
        {
            musicSlider.value = data.musicVolume;
            SetMixerVolume("MusicVolume", data.musicVolume);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = data.sfxVolume;
            SetMixerVolume("SFXVolume", data.sfxVolume);
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log("TogglePause вызван, isPaused = " + isPaused);

        pausePanel.SetActive(isPaused);
        if (touchControls != null)
            touchControls.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        ApplySnapshot();
        PlayClick();
    }

    public void Resume()
    {
        isPaused = false;
        Debug.Log("Resume вызван, isPaused = " + isPaused);

        pausePanel.SetActive(false);
        if (touchControls != null)
            touchControls.SetActive(true);
        Time.timeScale = 1f;

        ApplySnapshot();
        PlayClick();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        PlayClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        PlayClick();
        SceneManager.LoadScene("MainMenu");
    }

    void ApplySnapshot()
    {
        Debug.Log("ApplySnapshot: mixer=" + (mixer != null) + " normal=" + (normalSnapshot != null) + " paused=" + (pausedSnapshot != null));

        if (mixer == null) return;

        if (isPaused && pausedSnapshot != null)
        {
            Debug.Log("Переключаюсь на Paused снапшот");
            pausedSnapshot.TransitionTo(snapshotTransition);
        }
        else if (!isPaused && normalSnapshot != null)
        {
            Debug.Log("Переключаюсь на Normal снапшот");
            normalSnapshot.TransitionTo(snapshotTransition);
        }
    }

    void PlayClick()
    {
        SFX.Play(uiSource, uiClickClip);
    }

    void OnMusicVolumeChanged(float value)
    {
        SetMixerVolume("MusicVolume", value);
        SaveData data = SaveManager.Load();
        data.musicVolume = value;
        SaveManager.Save(data);
    }

    void OnSfxVolumeChanged(float value)
    {
        SetMixerVolume("SFXVolume", value);
        SaveData data = SaveManager.Load();
        data.sfxVolume = value;
        SaveManager.Save(data);
    }

    void SetMixerVolume(string parameter, float sliderValue)
    {
        float dB = (sliderValue <= 0.0001f) ? -80f : Mathf.Log10(sliderValue) * 20f;
        mixer.SetFloat(parameter, dB);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject touchControls;
    public Slider volumeSlider;

    private bool isPaused;

    void Start()
    {
        SaveData data = SaveManager.Load();

        if (volumeSlider != null)
        {
            volumeSlider.value = data.musicVolume;
            AudioListener.volume = data.musicVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        if (touchControls != null)
            touchControls.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        if (touchControls != null)
            touchControls.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        SaveData data = SaveManager.Load();
        data.musicVolume = value;
        SaveManager.Save(data);
    }
}
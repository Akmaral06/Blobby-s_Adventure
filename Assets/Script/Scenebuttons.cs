using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtons : MonoBehaviour
{
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
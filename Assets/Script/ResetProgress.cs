using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgress : MonoBehaviour
{
    public GameObject confirmPanel;

    public void ShowConfirm()
    {
        confirmPanel.SetActive(true);
    }

    public void HideConfirm()
    {
        confirmPanel.SetActive(false);
    }

    public void ConfirmReset()
    {
        SaveData freshData = new SaveData();
        SaveManager.Save(freshData);
        confirmPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
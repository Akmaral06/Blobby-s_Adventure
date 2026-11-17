using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    public GameObject lockIcon2;
    public GameObject lockIcon3;

    void OnEnable()
    {
        SaveData data = SaveManager.Load();

        level1Button.interactable = true;

        bool level2Unlocked = data.unlockedLevels.Contains(2);
        level2Button.interactable = level2Unlocked;
        if (lockIcon2 != null) lockIcon2.SetActive(!level2Unlocked);

        bool level3Unlocked = data.unlockedLevels.Contains(3);
        level3Button.interactable = level3Unlocked;
        if (lockIcon3 != null) lockIcon3.SetActive(!level3Unlocked);
    }

    public void LoadLevel1() { SceneManager.LoadScene("Level1"); }
    public void LoadLevel2() { SceneManager.LoadScene("Level2"); }
    public void LoadLevel3() { SceneManager.LoadScene("Level3"); }
}
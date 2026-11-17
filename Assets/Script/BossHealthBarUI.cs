using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ConnectToBoss());
    }

    IEnumerator ConnectToBoss()
    {
        yield return null; 

        Slider slider = GetComponent<Slider>();
        BossHealth boss = FindFirstObjectByType<BossHealth>();

        if (boss != null && slider != null)
        {
            boss.hpBar = slider;
            slider.maxValue = boss.maxHp;
            slider.value = boss.hp;
            Debug.Log("HP-бар подключен к боссу, maxHp = " + boss.maxHp);
        }
        else
        {
            Debug.Log("Босс не найден! boss = " + boss + ", slider = " + slider);
        }
    }
}
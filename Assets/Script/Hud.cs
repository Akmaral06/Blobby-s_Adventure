using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public Player2D player;
    public Health health;
    public TMP_Text coinText;
    public TMP_Text hpText;            
    public GameObject[] hearts;        
    public int coinGoal = 3;           

    void Start()
    {
        if (player == null) player = FindFirstObjectByType<Player2D>();
        if (health == null && player != null) health = player.GetComponent<Health>();
    }

    void Update()
    {
        if (player == null || health == null) return;

        if (coinText != null)
            coinText.text = coinGoal > 0 ? player.coins + " / " + coinGoal : player.coins.ToString();

        if (hpText != null)
            hpText.text = "HP: " + health.hp;

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(i < health.hp);
    }
}
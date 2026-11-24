using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clickClip;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        SFX.Play(source, clickClip);
    }
}
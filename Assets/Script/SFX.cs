using UnityEngine;

public static class SFX
{
    public static void Play(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.pitch = Random.Range(0.92f, 1.08f);
        source.PlayOneShot(clip);
    }

    public static void Play(AudioSource source, AudioClip[] clips)
    {
        if (source == null || clips == null || clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        Play(source, clip);
    }
}
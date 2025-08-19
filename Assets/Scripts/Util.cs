using UnityEngine;

public class Util
{
    public static void PlaySFX(AudioSource audioSource, AudioClip audioClip = null, float pitchDelta = 0.1f)
    {
        if (audioSource == null) return;
        if (audioClip != null) audioSource.clip = audioClip; 
        audioSource.pitch = Random.Range(1.00f - pitchDelta, 1.00f + pitchDelta);
        audioSource.Play();
    }
}

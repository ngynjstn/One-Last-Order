using UnityEngine;
using System.Collections;

public class CafeAudioManager : MonoBehaviour
{
    public AudioSource cafeMusicSource;
    public AudioSource windSoundSource;
    public AudioReverbZone cafeReverbZone;
    public AudioReverbZone outsideReverbZone;
    public float fadeDuration = 2.0f;
    public float lowCafeVolumeOutside = 0.01f;
    public float outsideWindStartVolume = 0.5f;
    public float cafeMusicTargetVolume = 1.0f; 

    private bool playerInsideCafeZone = false;
    private float initialCafeVolume = 0.155f; //start volume?

    void Start()
    {
        // playing cafe music at the beginning with the specified initial volume
        if (cafeMusicSource != null && !cafeMusicSource.isPlaying)
        {
            cafeMusicSource.volume = initialCafeVolume;
            cafeMusicSource.Play();
        }
        else if (cafeMusicSource != null)
        {
            cafeMusicSource.volume = initialCafeVolume;
        }

        
        if (windSoundSource != null)
        {
            windSoundSource.volume = 0f;
            windSoundSource.Stop();
        }

        // nnable cafe reverb at the start
        if (cafeReverbZone != null)
        {
            cafeReverbZone.enabled = true;
        }
        if (outsideReverbZone != null)
        {
            outsideReverbZone.enabled = false; 
        }

        playerInsideCafeZone = true; // 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInsideCafeZone)
        {
            playerInsideCafeZone = true;

            // start playing cafe music and fade it in
            if (cafeMusicSource != null && !cafeMusicSource.isPlaying)
            {
                cafeMusicSource.Play();
                StartCoroutine(FadeAudio(cafeMusicSource, cafeMusicTargetVolume, fadeDuration));
            }
            else if (cafeMusicSource != null)
            {
                StartCoroutine(FadeAudio(cafeMusicSource, cafeMusicTargetVolume, fadeDuration));
            }

            // Fade out and stop wind sound
            if (windSoundSource != null && windSoundSource.isPlaying)
            {
                StartCoroutine(FadeOutAndStop(windSoundSource, fadeDuration));
            }

            // Enable cafe reverb
            if (cafeReverbZone != null)
            {
                cafeReverbZone.enabled = true;
            }
            // Disable outside reverb
            if (outsideReverbZone != null)
            {
                outsideReverbZone.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerInsideCafeZone)
        {
            playerInsideCafeZone = false;

            // 
            if (cafeMusicSource != null)
            {
                cafeMusicSource.volume = lowCafeVolumeOutside;
            }

            // y
            if (windSoundSource != null && !windSoundSource.isPlaying)
            {
                windSoundSource.volume = outsideWindStartVolume;
                windSoundSource.Play();
                StartCoroutine(FadeInAudio(windSoundSource, 1f, fadeDuration)); // Fade to full volume
            }
            else if (windSoundSource != null)
            {
                StartCoroutine(FadeInAudio(windSoundSource, 1f, fadeDuration)); // Fade in if already playing
            }

            // Disable cafe reverb
            if (cafeReverbZone != null)
            {
                cafeReverbZone.enabled = false;
            }
            // Enable outside reverb
            if (outsideReverbZone != null)
            {
                outsideReverbZone.enabled = true;
            }
        }
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        if (audioSource == null) yield break;
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeInAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        if (audioSource == null) yield break;
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        if (audioSource == null) yield break;
        float startVolume = audioSource.volume;
        float time = 0;
        float targetVolume = 0f;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
        audioSource.Stop();
    }
}
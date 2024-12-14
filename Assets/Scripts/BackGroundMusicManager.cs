using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public List<AudioClip> musicClips; // Lista de clipes de música
    private AudioSource audioSource;
    public static float volume = 1.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= volume;

        if (musicClips == null || musicClips.Count == 0)
        {
            Debug.LogError("BackgroundMusicManager: Lista de músicas está vazia!");
            return;
        }

        PlayRandomMusic();
    }

    void Update()
    {
        // Verifica se a música terminou de tocar
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    private void PlayRandomMusic()
    {
        // Escolhe um clipe aleatório
        AudioClip selectedClip = musicClips[Random.Range(2, musicClips.Count)];
        audioSource.clip = selectedClip;

        // Reproduz a música
        audioSource.Play();

        Debug.Log($"Tocando: {selectedClip.name}");
    }
}

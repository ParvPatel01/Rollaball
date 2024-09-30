using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource backgroundMusicSource;  // Reference to the AudioSource
    public AudioClip backgroundMusic;  // Reference to the background music

    void Start()
    {
        // Set the background music clip
        if (backgroundMusicSource != null && backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
        }
        
        // Automatically play the background music on start
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    // Method to pause the music
    public void PauseMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Pause();
        }
    }

    // Method to resume the music
    public void ResumeMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }

    // Method to stop the music
    public void StopMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }
}

using UnityEngine;

public class MusicScript : MonoBehaviour
{
    const float MULTIPLIER = 0.087f;
    AudioSource backgroundMusic;

    void Awake()
    {
        backgroundMusic = GetComponent<AudioSource>();

        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        backgroundMusic.volume = Globals.audioSettings.musicVolume * MULTIPLIER;
    }
}

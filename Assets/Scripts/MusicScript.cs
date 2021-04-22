using UnityEngine;

public class MusicScript : MonoBehaviour
{
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
        backgroundMusic.volume = Globals.audioSettings.musicVolume;
    }
}

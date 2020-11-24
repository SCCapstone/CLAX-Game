using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistMusicThroughReset : MonoBehaviour
{
    AudioSource backgroundMusic;
    // Start is called before the first frame update
    void Awake()
    {
        backgroundMusic = GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        playMusic();
    }
    void playMusic()
    {
        if (backgroundMusic.isPlaying)
            return;
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

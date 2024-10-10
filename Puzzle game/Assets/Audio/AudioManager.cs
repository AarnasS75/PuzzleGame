using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Space(2)]
    [Header("SOUNDS")]
    public Sound[] sounds;

    public static AudioManager Instance;

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfxSource;
    private bool firstSourceIsPlaying;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        music1 = gameObject.AddComponent<AudioSource>();
        music2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(SoundTitle musicTitle)
    {
        var musicToPlay = Array.Find(sounds, sound => sound.Title == musicTitle);

        if (musicToPlay == null)
        {
            print($"Sound titled [{musicTitle}] was not found");
            return;
        }

        AudioSource activeSound = (firstSourceIsPlaying) ? music1 : music2;

        activeSound.clip = musicToPlay.Clip;
        activeSound.volume = musicToPlay.Volume;
        activeSound.loop = musicToPlay.IsLooping;
        activeSound.Play();
    }

    public void PlaySFX(SoundTitle sfxTitle)
    {
        Sound sfx = Array.Find(sounds, sound => sound.Title == sfxTitle);

        sfxSource.PlayOneShot(sfx.Clip, sfx.Volume);
    }

    public void SetMusicVolume(float volume)
    {
        music1.volume = volume;
        music2.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

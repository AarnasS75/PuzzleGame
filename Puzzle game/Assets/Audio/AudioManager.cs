using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Space(2)]
    [Header("SFX")]
    public Sfx[] _sfxs;

    [Space(2)]
    [Header("SONGS")]
    public Song[] _songs;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    protected override void Awake()
    {
        base.Awake();

        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Play(SfxTitle.Humming);
            Play(SfxTitle.LightsBuzz);
        }
    }

    public void Play(SongTitle musicTitle)
    {
        var musicToPlay = Array.Find(_songs, sound => sound.Title == musicTitle);

        if (musicToPlay == null)
        {
            print($"Sound titled [{musicTitle}] was not found");
            return;
        }

        _musicSource.clip = musicToPlay.Clip;
        _musicSource.volume = musicToPlay.Volume;
        _musicSource.loop = musicToPlay.IsLooping;
        _musicSource.Play();
    }

    public void Play(SfxTitle sfxTitle)
    {
        var sfx = Array.Find(_sfxs, fx => fx.Title == sfxTitle);

        if (sfx == null)
        {
            print($"Sfx titled [{sfxTitle}] was not found");
            return;
        }

        _sfxSource.clip = sfx.Clip;
        _sfxSource.volume = sfx.Volume;
        _sfxSource.loop = sfx.IsLooping;
        _sfxSource.PlayOneShot(sfx.Clip, sfx.Volume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}

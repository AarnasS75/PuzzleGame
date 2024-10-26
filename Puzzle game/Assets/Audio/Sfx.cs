using System;
using UnityEngine;

[Serializable]
public class Sfx : Sound
{
    public SfxTitle Title;
}

[Serializable]
public class Song : Sound
{
    public SongTitle Title;
}

public abstract class Sound
{
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;

    public bool IsLooping;

    [HideInInspector]
    public AudioSource Source;
}
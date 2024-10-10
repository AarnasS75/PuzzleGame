using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip Clip;
    public SoundTitle Title;

    [Range(0f, 1f)]
    public float Volume;

    public bool IsLooping;

    [HideInInspector]
    public AudioSource Source;
}

public enum SoundTitle
{
    MachineryHumming,
    AirVents,
    Footsteps,
    ElectricalBuzz
}
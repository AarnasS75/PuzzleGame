using UnityEngine;

public class VolumeController : OptionsController<float>
{
    private void Awake()
    {
        _optionsKey = Settings.VOLUME_VALUE;
    }

    public override void ApplySetting(float value)
    {
        AudioListener.volume = value;
    }

    protected override void Save()
    {
        PlayerPrefs.SetFloat(Settings.VOLUME_VALUE, AudioListener.volume);
    }

    protected override void Load()
    {
        AudioListener.volume = PlayerPrefs.GetFloat(Settings.VOLUME_VALUE);
    }
}

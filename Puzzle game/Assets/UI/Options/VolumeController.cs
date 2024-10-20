using UnityEngine;
using UnityEngine.UI;

public class VolumeController : OptionsController<float>
{
    [SerializeField] private Slider _volumeSlider;

    public override void ApplySetting(float value)
    {
        AudioListener.volume = value;
    }

    public override void Save()
    {
        PlayerPrefs.SetFloat(Settings.VOLUME_VALUE, AudioListener.volume);
        PlayerPrefs.Save();
    }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(Settings.VOLUME_VALUE))
        {
            _volumeSlider.value = AudioListener.volume = PlayerPrefs.GetFloat(Settings.VOLUME_VALUE);
        }
        else
        {
            Save();
        }
    }
}

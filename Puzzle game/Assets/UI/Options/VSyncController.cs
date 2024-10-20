using UnityEngine;
using UnityEngine.UI;

public class VSyncController : OptionsController<bool>
{
    [SerializeField] private Toggle _toggle;

    public override void ApplySetting(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    public override void Save()
    {
        PlayerPrefs.SetInt(Settings.VSYNC_VALUE, QualitySettings.vSyncCount);

        PlayerPrefs.Save();
    }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(Settings.VSYNC_VALUE))
        {
            QualitySettings.vSyncCount = PlayerPrefs.GetInt(Settings.VSYNC_VALUE, 0);

            _toggle.isOn = QualitySettings.vSyncCount == 1;
        }
        else
        {
            Save();
        }
    }
}

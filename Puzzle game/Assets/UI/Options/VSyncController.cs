using UnityEngine;

public class VSyncController : OptionsController<bool>
{
    private void Awake()
    {
        _optionsKey = Settings.VSYNC_VALUE;
    }

    public override void ApplySetting(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;

        Save();
    }

    protected override void Save()
    {
        PlayerPrefs.SetInt(Settings.VSYNC_VALUE, QualitySettings.vSyncCount);

        PlayerPrefs.Save();
    }

    protected override void Load()
    {
        int vsyncSetting = PlayerPrefs.GetInt(Settings.VSYNC_VALUE, 0);

        QualitySettings.vSyncCount = vsyncSetting;
    }
}

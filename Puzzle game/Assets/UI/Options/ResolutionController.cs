using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionController : OptionsController<int>
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private void Awake()
    {
        _optionsKey = Settings.RESOLUTION_X_VALUE;
    }

    protected override void Start()
    {
        base.Start();

        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            if ((float)_resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        _filteredResolutions.Sort((a, b) =>
        {
            if (a.width != b.width)
                return b.width.CompareTo(a.width);
            else
                return b.height.CompareTo(a.height);
        });

        var options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height;
            options.Add(resolutionOption);

            if (_filteredResolutions[i].width == Screen.width &&
                _filteredResolutions[i].height == Screen.height &&
                (float)_filteredResolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex = 0;
        resolutionDropdown.RefreshShownValue();
    }

    public override void ApplySetting(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    protected override void Save()
    {
        PlayerPrefs.SetInt(Settings.RESOLUTION_X_VALUE, Screen.currentResolution.width);
        PlayerPrefs.SetInt(Settings.RESOLUTION_Y_VALUE, Screen.currentResolution.height);

        PlayerPrefs.Save();
    }

    protected override void Load()
    {
        var resX = PlayerPrefs.GetInt(Settings.RESOLUTION_X_VALUE);
        var resY = PlayerPrefs.GetInt(Settings.RESOLUTION_Y_VALUE);

        Screen.SetResolution(resX, resY, true);
    }
}

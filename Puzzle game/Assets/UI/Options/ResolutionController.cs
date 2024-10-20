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

    public void InitializeDropdown()
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

        PlayerPrefs.SetInt(Settings.RESOLUTION_X_VALUE, resolution.width);
        PlayerPrefs.SetInt(Settings.RESOLUTION_Y_VALUE, resolution.height);

        Save();
    }

    public override void Save()
    {
        PlayerPrefs.Save();
    }

    public override void Load()
    {
        // Check if the resolution settings are stored in PlayerPrefs
        if (PlayerPrefs.HasKey(Settings.RESOLUTION_X_VALUE) && PlayerPrefs.HasKey(Settings.RESOLUTION_Y_VALUE))
        {
            var resX = PlayerPrefs.GetInt(Settings.RESOLUTION_X_VALUE);
            var resY = PlayerPrefs.GetInt(Settings.RESOLUTION_Y_VALUE);

            // Find the index of the saved resolution in the filtered resolutions list
            int savedResolutionIndex = _filteredResolutions.FindIndex(res =>
                res.width == resX && res.height == resY &&
                (float)res.refreshRateRatio.value == currentRefreshRate);

            // If the resolution is found, set the dropdown value to the corresponding index
            if (savedResolutionIndex != -1)
            {
                resolutionDropdown.value = savedResolutionIndex;
                resolutionDropdown.RefreshShownValue();

                // Optionally, set the screen resolution to match the saved resolution
                Screen.SetResolution(resX, resY, true);
            }
            else
            {
                // If resolution not found, revert to the current resolution or default behavior
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
            }
        }
        else
        {
            // If no resolution is saved, set the current resolution as default and save it
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            Save();  // Save the current resolution to PlayerPrefs
        }
    }

}

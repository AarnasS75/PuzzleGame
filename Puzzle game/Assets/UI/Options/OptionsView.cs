using UnityEngine;

public class OptionsView : View
{
    [SerializeField] private ResolutionController _resolutionController;
    [SerializeField] private VSyncController _vSyncController;
    [SerializeField] private VolumeController _volumeController;

    public override void Initialize()
    {
        _resolutionController.InitializeDropdown();
        _resolutionController.Load();
        _vSyncController.Load();
        _volumeController.Load();
    }

    private void OnEnable()
    {
        _resolutionController.Load();
        _vSyncController.Load();
        _volumeController.Load();
    }

    private void OnDisable()
    {
        _resolutionController.Save();
        _vSyncController.Save();
        _volumeController.Save();
    }

    public void GoBack()
    {
        InputManager.CallEscapeButtonPressed();
    }
}
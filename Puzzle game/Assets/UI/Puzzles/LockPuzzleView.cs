using UnityEngine;
using UnityEngine.UI;

public class LockPuzzleView : View
{
    [SerializeField] private Image _lockDial;
    [SerializeField] private float _rotationSpeed = 100f;

    private void OnEnable()
    {
        InputManager.OnScrollWheelPerformed += InputManager_OnScrollWheelPerformed;
    }

    private void OnDisable()
    {
        InputManager.OnScrollWheelPerformed -= InputManager_OnScrollWheelPerformed;
    }

    private void InputManager_OnScrollWheelPerformed(float scrollInput)
    {
        HandleScrollWheelInput(scrollInput);
    }

    private void HandleScrollWheelInput(float scrollInput)
    {
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            RotateDial(scrollInput * _rotationSpeed);
        }
    }

    private void RotateDial(float deltaRotation)
    {
        float newRotation = _lockDial.rectTransform.localEulerAngles.z + deltaRotation * _rotationSpeed * Time.deltaTime;

        newRotation = Mathf.Repeat(newRotation, 360f);

        _lockDial.rectTransform.localEulerAngles = new Vector3(0, 0, newRotation);
    }
}

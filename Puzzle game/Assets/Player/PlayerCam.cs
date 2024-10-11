using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private Transform _playerCamera;

    private const float CLAMP = 90;
    private float xRotation = 0f;

    private float mouseX, mouseY;

    private void OnEnable()
    {
        InputManager.OnLookPerformed += InputManager_OnLookPerformed;
    }

    private void OnDisable()
    {
        InputManager.OnLookPerformed -= InputManager_OnLookPerformed;
    }

    private void Update()
    {
        if (!InputManager.MovementEnabled)
        {
            mouseX = 0f;
            mouseY = 0f;
            return;
        }

        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -CLAMP, CLAMP);
        var targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        _playerCamera.eulerAngles = targetRotation;
    }

    private void InputManager_OnLookPerformed(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * _sensX;
        mouseY = mouseInput.y * _sensY;
    }
}

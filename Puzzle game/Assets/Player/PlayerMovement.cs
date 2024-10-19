using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _gravity = 10f;

    private Vector2 _input;
    private float _verticalVelocity;

    private void OnEnable()
    {
        InputManager.OnMovementPerformed += InputManager_OnMovementPerformed;
    }

    private void OnDisable()
    {
        InputManager.OnMovementPerformed -= InputManager_OnMovementPerformed;
    }

    private void InputManager_OnMovementPerformed(Vector2 input)
    {
        _input = input;
    }

    private void Update()
    {
        if (!InputManager.MovementEnabled)
        {
            _input = Vector2.zero;
            return;
        }

        var horizontalVelocity = (transform.right * _input.x + transform.forward * _input.y) * _moveSpeed;

        if (!_characterController.isGrounded)
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }

        var movement = horizontalVelocity + Vector3.up * _verticalVelocity;
        _characterController.Move(movement * Time.deltaTime);
    }
}

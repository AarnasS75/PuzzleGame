using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _gravity = 10f;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.5f; // Time between footsteps

    private float footstepTimer;
    private Vector2 _input;
    private float _verticalVelocity;
    private bool isMoving;
    private float movementThreshold = 0.3f;

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

        // Check if player is moving, and handle footstep sounds
        HandleFootsteps(horizontalVelocity);
    }

    private void HandleFootsteps(Vector3 horizontalVelocity)
    {
        // Determine if the player is moving based on the magnitude of the horizontal velocity
        isMoving = horizontalVelocity.magnitude > movementThreshold;

        // If player is moving and grounded, start the footstep timer
        if (isMoving && _characterController.isGrounded)
        {
            footstepTimer -= Time.deltaTime;

            // Only play the footstep sound when the timer reaches 0
            if (footstepTimer <= 0f)
            {
                AudioManager.Instance.Play(SfxTitle.Footsteps); // Play footstep sound
                footstepTimer = footstepInterval; // Reset the timer for the next footstep
            }
        }
        else
        {
            // Reset the footstep timer when the player is not moving
            footstepTimer = 0f;
        }
    }
}

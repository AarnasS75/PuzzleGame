using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockPuzzleView : View
{
    [Header("Dial Configuration")]
    [SerializeField] private int[] _numberSequence;
    [SerializeField] private Image _lockDial;

    [Header("Number Slots")]
    [SerializeField] private TextMeshProUGUI _slot1;
    [SerializeField] private TextMeshProUGUI _slot2;
    [SerializeField] private TextMeshProUGUI _slot3;

    private const int NUMBER_OF_POSITIONS = 10; // Numbers 0-9
    private const float ANGLE_PER_NUMBER = 36f; // 360 degrees / 10 numbers
    private const float SNAP_INCREMENT = 12f;   // Rotate in steps of 5 degrees
    private const float THRESHOLD = 5f;       // Allowed deviation for snapping

    private bool _isDragging = false;
    private Vector2 _lastMousePosition;

    private int[] _playerAttemptSequence;
    private int _currentSequenceIndex = 0;

    private bool _waitingForResult = false;

    private bool _isCompleted = false;

    public override void Hide()
    {
        base.Hide();

        if (_isCompleted)
        {
            return;
        }

        Reset();
    }

    public override void Initialize()
    {
        _playerAttemptSequence = new int[_numberSequence.Length];
        _currentSequenceIndex = 0;

        _slot1.text = "";
        _slot2.text = "";
        _slot3.text = "";
    }

    private void Update()
    {
        if (_isCompleted)
        {
            return;
        }

        HandleMouseInput();
        UpdateDragState();
    }

    private void HandleMouseInput()
    {
        if (_isDragging && !_waitingForResult)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            float angleDelta = GetAngleDelta(currentMousePosition);

            RotateDial(angleDelta);

            _lastMousePosition = currentMousePosition;
        }
    }

    private float GetAngleDelta(Vector2 currentMousePosition)
    {
        Vector2 dialCenter = _lockDial.rectTransform.position; // Get the center of the dial

        Vector2 lastDir = _lastMousePosition - dialCenter;
        Vector2 currentDir = currentMousePosition - dialCenter;

        // Get the angle difference between the last and current mouse position
        float lastAngle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;

        // Calculate the angular difference
        float angleDelta = Mathf.DeltaAngle(lastAngle, currentAngle);

        return angleDelta;
    }

    private void UpdateDragState()
    {
        if (_waitingForResult)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _lastMousePosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isDragging = false;
            CheckForExactPosition();
        }
    }

    private void RotateDial(float deltaRotation)
    {
        float newRotation = _lockDial.rectTransform.localEulerAngles.z + deltaRotation;

        // Snap to the nearest increment
        newRotation = Mathf.Round(newRotation / SNAP_INCREMENT) * SNAP_INCREMENT;

        newRotation = Mathf.Repeat(newRotation, 360f);
        _lockDial.rectTransform.localEulerAngles = new Vector3(0, 0, newRotation);
    }

    private void CheckForExactPosition()
    {
        float currentRotation = _lockDial.rectTransform.localEulerAngles.z;

        // Calculate the current number from the rotation angle
        int currentNumber = -1;
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            float targetAngle = i * ANGLE_PER_NUMBER;
            if (Mathf.Abs(currentRotation - targetAngle) <= THRESHOLD)
            {
                currentNumber = i + 1;
                if (currentNumber == NUMBER_OF_POSITIONS)
                {
                    currentNumber = 0;
                }
                break;
            }
        }

        if (currentNumber == -1)
        {
            // If no exact number is found, return
            return;
        }

        // Store the player's input
        _playerAttemptSequence[_currentSequenceIndex] = currentNumber;
        _currentSequenceIndex++;

        // Update the UI slots
        if (string.IsNullOrEmpty(_slot1.text))
        {
            _slot1.text = currentNumber.ToString();
        }
        else if (string.IsNullOrEmpty(_slot2.text))
        {
            _slot2.text = currentNumber.ToString();
        }
        else if (string.IsNullOrEmpty(_slot3.text))
        {
            _slot3.text = currentNumber.ToString();
        }

        // If the player has entered all the numbers in the sequence
        if (_currentSequenceIndex >= _numberSequence.Length)
        {
            StartCoroutine(nameof(WaitForResult));
        }
    }

    private IEnumerator WaitForResult()
    {
        _waitingForResult = true;
        yield return new WaitForSeconds(1.5f);

        if (CheckIfSequenceIsCorrect())
        {
            _isCompleted = true;
            StaticEventsHandler.CallPuzzleCompletedEvent(this);
        }
        else
        {
            print("Incorrect combination. Try again.");
            ResetPlayerAttempt();
        }

        _waitingForResult = false;
    }

    private bool CheckIfSequenceIsCorrect()
    {
        // Compare the player's attempt with the correct sequence
        for (int i = 0; i < _numberSequence.Length; i++)
        {
            if (_playerAttemptSequence[i] != _numberSequence[i])
            {
                return false; // If any number doesn't match, return false
            }
        }
        return true; // All numbers match, return true
    }

    private void ResetPlayerAttempt()
    {
        _slot1.text = "";
        _slot2.text = "";
        _slot3.text = "";

        _currentSequenceIndex = 0; // Reset index
        // Optionally clear the player's input array if needed
        for (int i = 0; i < _playerAttemptSequence.Length; i++)
        {
            _playerAttemptSequence[i] = -1; // Clear previous attempts
        }
    }

    private void Reset()
    {
        _waitingForResult = false;
        _lockDial.rectTransform.localEulerAngles = Vector3.zero;
        ResetPlayerAttempt();
        StopAllCoroutines();
    }
}

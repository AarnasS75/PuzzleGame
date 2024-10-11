using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockPuzzleView : PuzzleView
{
    [Header("Dial Configuration")]
    [SerializeField] private Image _lockDial;

    [Header("Number Slots")]
    [SerializeField] private TextMeshProUGUI _slot1;
    [SerializeField] private TextMeshProUGUI _slot2;
    [SerializeField] private TextMeshProUGUI _slot3;

    private const int NumberOfPositions = 10; // Numbers 0-9
    private const float AnglePerNumber = 36f; // 360 degrees / 10 numbers
    private const float SnapIncrement = 12f;   // Rotate in steps of 5 degrees
    private const float Threshold = 5f;       // Allowed deviation for snapping
    private bool _isDragging = false;
    private Vector2 _lastMousePosition;

    private int[] _numberSequence;
    private int[] _playerAttemptSequence; // Stores the player's dial inputs
    private int _currentSequenceIndex = 0; // Tracks current position in sequence

    private void Update()
    {
        HandleMouseInput();
        UpdateDragState();
    }

    public void Initialize(int[] numberSequence)
    {
        _numberSequence = numberSequence;
        _playerAttemptSequence = new int[_numberSequence.Length]; // Initialize player attempt array
        _currentSequenceIndex = 0; // Reset sequence index

        _slot1.text = "";
        _slot2.text = "";
        _slot3.text = "";
    }

    private void HandleMouseInput()
    {
        if (_isDragging)
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
        newRotation = Mathf.Round(newRotation / SnapIncrement) * SnapIncrement;

        newRotation = Mathf.Repeat(newRotation, 360f);
        _lockDial.rectTransform.localEulerAngles = new Vector3(0, 0, newRotation);
    }

    private void CheckForExactPosition()
    {
        float currentRotation = _lockDial.rectTransform.localEulerAngles.z;

        // Calculate the current number from the rotation angle
        int currentNumber = -1;
        for (int i = 0; i < NumberOfPositions; i++)
        {
            float targetAngle = i * AnglePerNumber;
            if (Mathf.Abs(currentRotation - targetAngle) <= Threshold)
            {
                currentNumber = i + 1;
                if (currentNumber == NumberOfPositions)
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

        // Check if the player has finished entering the sequence
        if (_currentSequenceIndex >= _numberSequence.Length)
        {
            // Check if the player's attempt matches the correct sequence
            if (CheckIfSequenceIsCorrect())
            {
                print("Lock opened! The combination is correct.");
                StaticEventsHandler.CallPuzzleCompletedEvent(this);
            }
            else
            {
                print("Incorrect combination. Try again.");
                ResetPlayerAttempt();
            }
        }
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
}

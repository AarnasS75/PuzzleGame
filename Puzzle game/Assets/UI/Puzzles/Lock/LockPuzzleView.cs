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
    private const float THRESHOLD = 0.5f;       // Allowed deviation for snapping

    private int[] _playerAttemptSequence;
    private int _currentSequenceIndex = 0;

    private bool _waitingForResult = false;

    private bool _isCompleted = false;

    private void OnEnable()
    {
        // Subscribe to scroll input
        InputManager.OnScrollPerformed += HandleScrollInput;
    }

    private void OnDisable()
    {
        // Unsubscribe from scroll input
        InputManager.OnScrollPerformed -= HandleScrollInput;
    }

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

        UpdateDragState();
    }

    private void HandleScrollInput(float scrollValue)
    {
        if (_isCompleted || _waitingForResult) return;

        // Rotate the dial by a fixed angle increment based on scroll direction
        float rotationAmount = scrollValue > 0 ? ANGLE_PER_NUMBER : -ANGLE_PER_NUMBER;
        RotateDial(rotationAmount);
    }

    private void UpdateDragState()
    {
        if (_waitingForResult || _currentSequenceIndex >= _numberSequence.Length)
        {
            return;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            CheckForExactPosition();
        }
    }

    private void RotateDial(float deltaRotation)
    {
        float previousRotation = _lockDial.rectTransform.localEulerAngles.z;
        float newRotation = previousRotation + deltaRotation;
        newRotation = Mathf.Round(newRotation / SNAP_INCREMENT) * SNAP_INCREMENT;
        newRotation = Mathf.Repeat(newRotation, 360f);
        _lockDial.rectTransform.localEulerAngles = new Vector3(0, 0, newRotation);

        if (Mathf.Abs(newRotation - previousRotation) >= SNAP_INCREMENT)
        {
            AudioManager.Instance.Play(SfxTitle.SafeDial);
        }
    }

    private void CheckForExactPosition()
    {
        float currentRotation = _lockDial.rectTransform.localEulerAngles.z;

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

        AudioManager.Instance.Play(SfxTitle.CorrectSafeNumber);

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
            AudioManager.Instance.Play(SfxTitle.SafeOpen);
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

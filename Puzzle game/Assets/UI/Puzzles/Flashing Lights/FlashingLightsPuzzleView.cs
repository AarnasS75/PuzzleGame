using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLightsPuzzleView : View
{
    [SerializeField] private Transform _grid;
    [SerializeField] private int _totalPatterns = 5; // Total patterns player needs to complete
    [SerializeField] private float _timeLimit = 20f; // Time limit for each pattern
    [SerializeField] private float _timeBetweenFlashes = 1.2f; // Time limit for each pattern

    private LightSlot[] _slots;
    private List<int> _flashPattern;
    private int _currentStep;
    private int _patternsCompleted;
    private float _timer;

    private WaitForSeconds _delayBetweenFlashes;

    public static bool PlayerTurn;

    private void Awake()
    {
        _slots = _grid.GetComponentsInChildren<LightSlot>();
        _delayBetweenFlashes = new WaitForSeconds(_timeBetweenFlashes);

        foreach (var slot in _slots)
        {
            slot.OnSelected += Slot_OnSelected;
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (PlayerTurn)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Debug.Log("Time's up! You failed the pattern.");
                ResetPattern(); // Reset the current pattern
            }
        }
    }

    private void Slot_OnSelected(LightSlot selectedSlot)
    {
        int index = System.Array.IndexOf(_slots, selectedSlot);

        if (index == _flashPattern[_currentStep])
        {
            _currentStep++;

            if (_currentStep >= _flashPattern.Count)
            {
                Debug.Log("Pattern completed!");

                _patternsCompleted++;

                if (_patternsCompleted >= _totalPatterns)
                {
                    Debug.Log("All patterns completed! Puzzle solved.");
                    PuzzleCompleted();
                }
                else
                {
                    StartGame();
                }
            }
        }
        else
        {
            Debug.Log("Wrong pattern! Try again.");
            ResetPattern(); // Reset the current pattern if the wrong slot is selected
        }
    }

    private void StartGame()
    {
        _timer = _timeLimit; // Reset timer for each pattern
        PlayerTurn = false;
        StartCoroutine(nameof(FlashingLightsRoutine));
    }

    private void ResetPattern()
    {
        PlayerTurn = false;
        _currentStep = 0;
        StartGame(); // Restart the current pattern
    }

    private IEnumerator FlashingLightsRoutine()
    {
        yield return new WaitForSeconds(3); // Initial delay before starting the pattern

        StartNewPattern();
    }

    private void StartNewPattern()
    {
        _flashPattern = new List<int>();
        _currentStep = 0;
        _timer = _timeLimit; // Reset timer for new pattern

        // Generate a new random pattern
        int patternLength = UnityEngine.Random.Range(3, 6); // Length of the flashing pattern
        for (int i = 0; i < patternLength; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _slots.Length);
            _flashPattern.Add(randomIndex);
        }

        StartCoroutine(nameof(StartFlashingPattern));
    }

    private IEnumerator StartFlashingPattern()
    {
        foreach (int index in _flashPattern)
        {
            _slots[index].Flash();
            yield return _delayBetweenFlashes; // Adjust the delay between flashes if needed
        }

        PlayerTurn = true; // Player can start repeating the pattern
    }

    private void PuzzleCompleted()
    {
        Debug.Log("Puzzle is fully completed!");
        StaticEventsHandler.CallPuzzleCompletedEvent(this);
    }
}
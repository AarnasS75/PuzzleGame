using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLightsView : PuzzleView
{
    [SerializeField] private LightSlot[] _slots;

    private PatternManager _patternManager;

    private void Awake()
    {
        // Initialize Pattern Manager
        _patternManager = new PatternManager(_slots.Length);

        // Subscribe to slot selection events
        foreach (var slot in _slots)
        {
            slot.OnSelected += Slot_OnSelected;
        }
    }

    private void OnEnable()
    {
        StartGame();
    }

    private void Slot_OnSelected(LightSlot slot)
    {
        // Notify the PatternManager of the player's selection
        _patternManager.HandlePlayerSelection(Array.IndexOf(_slots, slot));
    }

    public void StartGame()
    {
        StartCoroutine(nameof(Wait));
        _patternManager.StartNewPattern(_slots);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
    }
}

public class PatternManager
{
    private List<int> _flashPattern;
    private int _currentStep;
    private int _gridSize;

    public PatternManager(int gridSize)
    {
        _gridSize = gridSize;
        _flashPattern = new List<int>();
    }

    public void StartNewPattern(LightSlot[] slots)
    {
        _flashPattern.Clear();
        _currentStep = 0;

        // Generate a new pattern
        int patternLength = UnityEngine.Random.Range(3, 6); // Length of the flashing pattern
        for (int i = 0; i < patternLength; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _gridSize);
            _flashPattern.Add(randomIndex);
        }

        // Start the flashing sequence
        StartFlashingPattern(slots);
    }

    private void StartFlashingPattern(LightSlot[] slots)
    {
        foreach (int index in _flashPattern)
        {
            slots[index].Flash(); // Flash the light slots in the pattern
            // Optional: Add delay between flashes
        }
    }

    public void HandlePlayerSelection(int index)
    {
        if (index == _flashPattern[_currentStep])
        {
            _currentStep++;

            if (_currentStep >= _flashPattern.Count)
            {
                Debug.Log("Pattern completed!");
                // Reset for next game or provide feedback
            }
        }
        else
        {
            Debug.Log("Wrong pattern! Try again.");
            // Optionally reset the pattern or provide feedback
        }
    }
}
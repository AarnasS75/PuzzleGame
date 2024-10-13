using UnityEngine;

public class PuzzlesController : MonoBehaviour
{
    [SerializeField] private LockPuzzleView _lockPuzzleView;
    [SerializeField] private SymbolMatchPuzzleView _symbolMatchPuzzleView;
    [SerializeField] private PipesPuzzleView _pipesPuzzleView;
    [SerializeField] private FlashingLightsView _flashingLightsView;

    private PuzzleView _activePuzzleView;

    private void Start()
    {
        _lockPuzzleView.Hide();
        _symbolMatchPuzzleView.Hide();
        _pipesPuzzleView.Hide();
        _flashingLightsView.Hide();
    }

    private void OnEnable()
    {
        InputManager.OnPuzzleObjectSelected += InputManager_OnPuzzleObjectSelected;
        InputManager.OnEscapePerformed += InputManager_OnEscapePerformed;

        StaticEventsHandler.OnPuzzleCompleted += StaticEventsHandler_OnPuzzleCompleted;
    }

    private void OnDisable()
    {
        InputManager.OnPuzzleObjectSelected -= InputManager_OnPuzzleObjectSelected;
        InputManager.OnEscapePerformed -= InputManager_OnEscapePerformed;

        StaticEventsHandler.OnPuzzleCompleted -= StaticEventsHandler_OnPuzzleCompleted;
    }

    private void InputManager_OnPuzzleObjectSelected(PuzzleObject obj)
    {
        switch (obj.Puzzle)
        {
            case Puzzle.Lock:
                _activePuzzleView = _lockPuzzleView;
                break;
            case Puzzle.MemoryTile:
                _activePuzzleView = _symbolMatchPuzzleView;
                break;
            case Puzzle.PipeConnection:
                _activePuzzleView = _pipesPuzzleView;
                break;
            case Puzzle.PatternSequence:
                _activePuzzleView = _flashingLightsView;
                break;
        }
        _activePuzzleView.Show();
    }

    private void InputManager_OnEscapePerformed()
    {
        if (_activePuzzleView != null)
        {
            ResetView();
        }
    }

    private void StaticEventsHandler_OnPuzzleCompleted(PuzzleView puzzleView)
    {
        ResetView();
    }

    private void ResetView()
    {
        _activePuzzleView.Hide();
        _activePuzzleView = null;
    }
}
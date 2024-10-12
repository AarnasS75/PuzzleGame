using UnityEngine;

public class PuzzlesController : MonoBehaviour
{
    [SerializeField] private LockPuzzleView _lockPuzzleView;
    [SerializeField] private SymbolMatchPuzzleView _symbolMatchPuzzleView;
    private PuzzleView _activePuzzleView;
    private PuzzleObject _activePuzzleObject;

    private void Start()
    {
        _lockPuzzleView.Hide();
        _symbolMatchPuzzleView.Hide();
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
        _activePuzzleObject = obj;

        switch (obj.Puzzle)
        {
            case Puzzle.Lock:
                _activePuzzleView = _lockPuzzleView;
                break;
            case Puzzle.MemoryTile:
                _activePuzzleView = _symbolMatchPuzzleView;
                break;
            case Puzzle.PipeConnection:
                break;
            case Puzzle.PatternSequence:
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
using UnityEngine;

public class PuzzlesView : View
{
    [SerializeField] private LockPuzzleView _lockPuzzleView;
    [SerializeField] private SymbolMatchPuzzleView _symbolMatchPuzzleView;
    [SerializeField] private PipesPuzzleView _pipesPuzzleView;
    [SerializeField] private FlashingLightsView _flashingLightsView;

    private View _activePuzzleView;

    private void Start()
    {
        _lockPuzzleView.Hide();
        _symbolMatchPuzzleView.Hide();
        _pipesPuzzleView.Hide();
        _flashingLightsView.Hide();
    }

    private void OnEnable()
    {
        StaticEventsHandler.OnPuzzleCompleted += StaticEventsHandler_OnPuzzleCompleted;
    }

    private void OnDisable()
    {
        StaticEventsHandler.OnPuzzleCompleted -= StaticEventsHandler_OnPuzzleCompleted;
    }

    public void ShowPuzzle(PuzzleObject obj)
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

    public void ClosePuzzle()
    {
        if (_activePuzzleView != null)
        {
            ResetView();
        }
    }

    private void StaticEventsHandler_OnPuzzleCompleted(View puzzleView)
    {
        ResetView();
    }

    private void ResetView()
    {
        _activePuzzleView.Hide();
        _activePuzzleView = null;
    }
}
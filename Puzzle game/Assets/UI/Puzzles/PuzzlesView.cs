using System.Linq;
using UnityEngine;

public class PuzzlesView : View
{
    [SerializeField] private View[] _puzzlesViews;

    private View _activePuzzleView;

    public override void Initialize()
    {
        for (int i = 0; i < _puzzlesViews.Length; i++)
        {
            _puzzlesViews[i].Initialize();
            _puzzlesViews[i].Hide();
        }
    }

    public void ShowPuzzle(PuzzleObject obj)
    {
        switch (obj.Puzzle)
        {
            case Puzzle.Lock:
                _activePuzzleView = _puzzlesViews.FirstOrDefault(x => x.GetType() == typeof(LockPuzzleView));
                break;
            case Puzzle.MemoryTile:
                _activePuzzleView = _puzzlesViews.FirstOrDefault(x => x.GetType() == typeof(SymbolMatchPuzzleView));
                break;
            case Puzzle.PipeConnection:
                _activePuzzleView = _puzzlesViews.FirstOrDefault(x => x.GetType() == typeof(PipesPuzzleView));
                break;
            case Puzzle.PatternSequence:
                _activePuzzleView = _puzzlesViews.FirstOrDefault(x => x.GetType() == typeof(FlashingLightsPuzzleView));
                break;
        }
        _activePuzzleView.Show();
    }

    public void ResetView()
    {
        _activePuzzleView.Hide();
        _activePuzzleView = null;
    }
}
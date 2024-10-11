using System.Linq;
using UnityEngine;

public class PuzzlesController : MonoBehaviour
{
    [SerializeField] private View[] _puzzleViews;
    private View _activePuzzleView;

    private void Start()
    {
        foreach (var puzzleView in _puzzleViews)
        {
            puzzleView.Hide();
        }
    }

    private void OnEnable()
    {
        InputManager.OnPuzzleObjectSelected += InputManager_OnPuzzleObjectSelected;
        InputManager.OnEscapePerformed += InputManager_OnEscapePerformed;
    }

    private void OnDisable()
    {
        InputManager.OnPuzzleObjectSelected -= InputManager_OnPuzzleObjectSelected;
        InputManager.OnEscapePerformed -= InputManager_OnEscapePerformed;
    }

    private void InputManager_OnPuzzleObjectSelected(PuzzleObject obj)
    {
        switch (obj.Puzzle)
        {
            case Puzzle.Lock:
                _activePuzzleView = _puzzleViews.FirstOrDefault(x => x.GetType() == typeof(LockPuzzleView));
                break;
            case Puzzle.MemoryTile:
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
            _activePuzzleView.Hide();
            _activePuzzleView = null;
        }
    }
}
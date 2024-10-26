using UnityEngine;

public class GameplayViewController : MonoBehaviour
{
    [SerializeField] private View _startingView;
    [SerializeField] private View[] _views;

    private View _currentView;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        InputManager.OnPuzzleObjectSelected += InputManager_OnPuzzleObjectSelected;
        InputManager.OnEndPuzzleObjectSelected += InputManager_OnEndPuzzleObjectSelected;
        InputManager.OnEscapePerformed += InputManager_OnEscapePerformed;
        StaticEventsHandler.OnPuzzleCompleted += StaticEventsHandler_OnPuzzleCompleted;
    }

    private void OnDisable()
    {
        InputManager.OnPuzzleObjectSelected -= InputManager_OnPuzzleObjectSelected;
        InputManager.OnEndPuzzleObjectSelected -= InputManager_OnEndPuzzleObjectSelected;
        InputManager.OnEscapePerformed -= InputManager_OnEscapePerformed;
        StaticEventsHandler.OnPuzzleCompleted -= StaticEventsHandler_OnPuzzleCompleted;
    }

    private void InputManager_OnEndPuzzleObjectSelected(EndPuzzle endPuzzleObj, int numberPressed)
    {
        var hudView = GetTab<HudView>();

        if (hudView.TryUsePuzzlePiece(numberPressed))
        {
            endPuzzleObj.AddPuzzlePiece();
        }
    }

    private void InputManager_OnEscapePerformed()
    {
        if (_currentView is OptionsView)
        {
            Show<HudView>();
        }
        else if (_currentView is HudView)
        {
            Show<OptionsView>();
        }
        else if (_currentView is PuzzlesView)
        {
            GetTab<PuzzlesView>().ResetView();
            Show<HudView>();
        }
    }

    private void StaticEventsHandler_OnPuzzleCompleted(View puzzleView)
    {
        GetTab<PuzzlesView>().ResetView();
        GetTab<HudView>().UpdateHud(puzzleView);

        Show<HudView>();
    }

    private void InputManager_OnPuzzleObjectSelected(PuzzleObject obj)
    {
        Show<PuzzlesView>();
        GetTab<PuzzlesView>().ShowPuzzle(obj);
    }

    private void Initialize()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();
        }

        if (_startingView != null)
        {
            Show(_startingView);
        }
    }

    private T GetTab<T>() where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T tTab)
            {
                return tTab;
            }
        }
        return null;
    }

    private void Show<T>() where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T)
            {
                if (_currentView != null)
                {
                    _currentView.Hide();
                }

                _views[i].Show();

                _currentView = _views[i];
            }
        }
    }

    private void Show(View tab)
    {
        if (_currentView != null)
        {
            _currentView.Hide();
        }
        tab.Show();

        _currentView = tab;
    }
}

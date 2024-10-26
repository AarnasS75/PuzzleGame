public class GameplayViewController : ViewController
{
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
        var hudView = Get<HudView>();

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
            Get<PuzzlesView>().ResetView();
            Show<HudView>();
        }
    }

    private void StaticEventsHandler_OnPuzzleCompleted(View puzzleView)
    {
        Get<PuzzlesView>().ResetView();
        Get<HudView>().UpdateHud(puzzleView);

        Show<HudView>();
    }

    private void InputManager_OnPuzzleObjectSelected(PuzzleObject obj)
    {
        Show<PuzzlesView>();
        Get<PuzzlesView>().ShowPuzzle(obj);
    }
}

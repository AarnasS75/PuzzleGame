using System;

public static class StaticEventsHandler
{
    public static event Action<View> OnPuzzleCompleted;
    public static event Action OnGameFinished;

    public static void CallPuzzleCompletedEvent(View puzzleView)
    {
        OnPuzzleCompleted?.Invoke(puzzleView);
    }

    public static void CallGameFinishedEvent()
    {
        OnGameFinished?.Invoke();
    }
}

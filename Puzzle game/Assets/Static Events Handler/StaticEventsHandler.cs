using System;

public static class StaticEventsHandler
{
    public static event Action<View> OnPuzzleCompleted;

    public static void CallPuzzleCompletedEvent(View puzzleView)
    {
        OnPuzzleCompleted?.Invoke(puzzleView);
    }
}

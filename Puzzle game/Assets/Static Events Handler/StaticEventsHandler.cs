using System;

public static class StaticEventsHandler
{
    public static event Action<PuzzleView> OnPuzzleCompleted;

    public static void CallPuzzleCompletedEvent(PuzzleView puzzleView)
    {
        OnPuzzleCompleted?.Invoke(puzzleView);
    }
}

using UnityEngine;

public class EndPuzzle : MonoBehaviour
{
    private int _piecesPlaced = 0;

    public void AddPuzzlePiece()
    {
        _piecesPlaced++;

        if (_piecesPlaced == 4)
        {
            StaticEventsHandler.CallGameFinishedEvent();
        }
    }
}

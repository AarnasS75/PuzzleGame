using System.Collections;
using System.Linq;
using UnityEngine;

public class PipesPuzzleView : PuzzleView
{
    [Header("Grid")]
    [SerializeField] private Transform _grid;

    [Header("Path")]
    [SerializeField] private PipeSlot[] _path;

    [Header("Sprites")]
    [SerializeField] private Sprite _straightPipeSprite;
    [SerializeField] private Sprite _turnPipeSprite;

    private PipeSlot[] _pipeSlots;

    private int _currentIndex = 0;
    private bool _isCheckingPath = false;

    private void Awake()
    {
        _pipeSlots = _grid.GetComponentsInChildren<PipeSlot>();
    }

    private void Start()
    {
        InitializeGrid();
        StartCoroutine(nameof(CheckPathStepByStep));
    }

    private void InitializeGrid()
    {
        foreach (var slot in _pipeSlots)
        {
            if (_path.Any(x => x.name == slot.transform.name))
            {
                continue;
            }

            var randomSprite = UnityEngine.Random.value > 0.5f ? _straightPipeSprite : _turnPipeSprite;
            slot.Initialize(randomSprite, PipeType.LeftToRight);
        }

        foreach (var pathPart in _path)
        {
            switch (pathPart.Type)
            {
                case PipeType.UpToLeft:
                case PipeType.UpToRight:
                case PipeType.DownToRight:
                case PipeType.DownToLeft:
                    pathPart.Initialize(_turnPipeSprite, pathPart.Type);
                    break;

                case PipeType.LeftToRight:
                case PipeType.UpToDown:
                    pathPart.Initialize(_straightPipeSprite, pathPart.Type);
                    break;
            }
            pathPart.Image.color = Color.red;
        }
    }

    private IEnumerator CheckPathStepByStep()
    {
        yield return new WaitForSeconds(10);

        // Start checking path step by step if it's not already running
        _isCheckingPath = true;

        while (_isCheckingPath && _currentIndex < _path.Length - 1)
        {
            // Check the current pipe with the next one
            var currentPipe = _path[_currentIndex];
            var nextPipe = _path[_currentIndex + 1];

            // Check if the current pipe connects correctly to the next pipe
            if (!currentPipe.IsConnectedCorrectly(nextPipe.Type))
            {
                Debug.LogError($"Pipe at index {_currentIndex} does not connect correctly to pipe at index {_currentIndex + 1}.");
                _isCheckingPath = false;
                yield break; // Stop the coroutine if there's an error
            }

            // Move to the next pipe after checking this one
            _currentIndex++;

            // Wait for the next frame before continuing (or a delay can be added here)
            yield return null;
        }

        // Check if the last pipe is marked as the end
        if (_currentIndex == _path.Length - 1 && !_path[_currentIndex].IsEnd)
        {
            Debug.LogError("The last pipe is not marked as the end pipe.");
        }
        else if (_currentIndex == _path.Length - 1)
        {
            Debug.Log("All pipes are connected correctly.");
        }

        _isCheckingPath = false; // End checking once finished
    }

}
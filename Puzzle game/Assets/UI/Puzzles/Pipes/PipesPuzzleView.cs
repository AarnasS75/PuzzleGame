using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipesPuzzleView : View
{
    [Header("Grid")]
    [SerializeField] private Transform _grid;
    [SerializeField] private PipeType _startConnectionRequired;
    [SerializeField] private PipeType _endConnectionRequired;

    [Header("Sprites")]
    [SerializeField] private Sprite _straightPipeSprite;
    [SerializeField] private Sprite _turnPipeSprite;

    private PipeSlot[] _pipeSlots;

    private void Awake()
    {
        _pipeSlots = _grid.GetComponentsInChildren<PipeSlot>();
    }

    private void Start()
    {
        InitializeGrid();
        StartCheckingPath();
    }

    private void InitializeGrid()
    {
        foreach (var slot in _pipeSlots)
        {
            switch (slot.Type)
            {
                case PipeType.None:
                    InitializeRandomSlot(slot);
                    break;

                case PipeType.Horizontal:
                case PipeType.Vertical:
                    slot.Initialize(_straightPipeSprite);
                    break;
                case PipeType.UpToLeft:
                case PipeType.UpToRight:
                case PipeType.DownToLeft:
                case PipeType.DownToRight:
                    slot.Initialize(_turnPipeSprite);
                    break;
            }
        }
    }

    private void StartCheckingPath()
    {
        StartCoroutine(CheckPathStepByStepWithDFS());
    }

    private void InitializeRandomSlot(PipeSlot slot)
    {
        if (Random.value > 0.5f)
        {
            slot.Initialize(_straightPipeSprite);
            slot.SetType(PipeType.Horizontal);
        }
        else
        {
            slot.Initialize(_turnPipeSprite);
            slot.SetType(PipeType.DownToLeft);
        }
    }

    private IEnumerator CheckPathStepByStepWithDFS()
    {
        yield return new WaitForSeconds(10);

        DisableSlotsButtons();

        var startPipe = _pipeSlots.FirstOrDefault(slot => slot.IsStart);
        var endPipe = _pipeSlots.FirstOrDefault(slot => slot.IsEnd);

        if (startPipe == null || endPipe == null)
        {
            Debug.LogError("Start or End pipe is not assigned.");
            yield break;
        }

        // Check if the start and end slots are of the correct types
        if (startPipe.Type != _startConnectionRequired)
        {
            startPipe.Image.color = Color.red;
            Debug.LogError("Start pipe is not correct type");
            yield break;
        }

        startPipe.Image.color = Color.green;
        var visited = new HashSet<PipeSlot>();

        // Start the DFS with the first slot
        yield return StartCoroutine(DFS(startPipe, endPipe, visited));
    }

    private IEnumerator DFS(PipeSlot current, PipeSlot end, HashSet<PipeSlot> visited)
    {
        visited.Add(current);

        yield return new WaitForSeconds(1f);

        if (current == end)
        {
            if (end.Type == _endConnectionRequired)
            {
                Debug.Log("Puzzle Completed!");
                StaticEventsHandler.CallPuzzleCompletedEvent(this);
            }
            else
            {
                end.Image.color = Color.red;
                Debug.LogError("End pipe is not correct type");
            }
            yield break;
        }

        // Get the neighboring pipes (up, down, left, right)
        var neighbors = GetNeighbors(current);

        foreach (var neighbor in neighbors)
        {
            Vector2 connectionDirection = (neighbor.transform.position - current.transform.position).normalized;

            if (!visited.Contains(neighbor) && current.IsConnectedCorrectly(neighbor.Type, connectionDirection))
            {
                neighbor.Image.color = Color.green;
                yield return StartCoroutine(DFS(neighbor, end, visited)); // Recursively check the neighbor
            }
        }
    }

    // Get neighbors by checking relative position using RectTransform
    private List<PipeSlot> GetNeighbors(PipeSlot pipeSlot)
    {
        List<PipeSlot> neighbors = new List<PipeSlot>();
        RectTransform currentRect = pipeSlot.GetComponent<RectTransform>();

        foreach (var slot in _pipeSlots)
        {
            if (slot == pipeSlot) continue;

            RectTransform otherRect = slot.GetComponent<RectTransform>();

            // Calculate the distance between the current and other slots
            Vector2 offset = (Vector2)otherRect.anchoredPosition - currentRect.anchoredPosition;

            // Check for horizontal or vertical neighbors (distance of 150 in one direction, 0 in the other)
            if (Mathf.Approximately(offset.x, 150f) && Mathf.Approximately(offset.y, 0f))
            {
                // Right neighbor
                neighbors.Add(slot);
            }
            else if (Mathf.Approximately(offset.x, -150f) && Mathf.Approximately(offset.y, 0f))
            {
                // Left neighbor
                neighbors.Add(slot);
            }
            else if (Mathf.Approximately(offset.x, 0f) && Mathf.Approximately(offset.y, 150f))
            {
                // Up neighbor
                neighbors.Add(slot);
            }
            else if (Mathf.Approximately(offset.x, 0f) && Mathf.Approximately(offset.y, -150f))
            {
                // Down neighbor
                neighbors.Add(slot);
            }
        }

        return neighbors;
    }

    private void DisableSlotsButtons()
    {
        foreach (var pipeSlot in _pipeSlots)
        {
            pipeSlot.DisableButton();
        }
    }
}
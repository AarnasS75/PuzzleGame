using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PipesPuzzleView : View
{
    [Header("Grid")]
    [SerializeField] private Transform _grid;
    [SerializeField] private PipeType _startConnectionRequired;
    [SerializeField] private PipeType _endConnectionRequired;

    [Header("Sprites")]
    [SerializeField] private Sprite _straightPipeSprite;
    [SerializeField] private Sprite _turnPipeSprite;

    [Header("Liquid")]
    [SerializeField] private Image _topLiquid;
    [SerializeField] private Image _botLiquid;
    [SerializeField] private float _fillDuration = 3f;

    private PipeSlot[] _pipeSlots;

    private bool _isCompleted = false;

    public override void Initialize()
    {
        InitializeGrid();
    }

    public override void Show()
    {
        base.Show();

        if (_isCompleted)
        {
            _botLiquid.fillAmount = 1;
            return;
        }

        StartPuzzle();
    }

    public override void Hide()
    {
        base.Hide();

        if (_isCompleted)
        {
            return;
        }

        Reset();
    }

    private void InitializeGrid()
    {
        _pipeSlots = _grid.GetComponentsInChildren<PipeSlot>();

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

    private void StartPuzzle()
    {
        _topLiquid.fillAmount = 0;
        _botLiquid.fillAmount = 0;
        StartCoroutine(FillTopLiquid());
    }

    private IEnumerator FillTopLiquid()
    {
        yield return new WaitForSeconds(5);

        float elapsedTime = 0f;

        while (elapsedTime < _fillDuration)
        {
            _topLiquid.fillAmount = Mathf.Lerp(0, 1, elapsedTime / _fillDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _topLiquid.fillAmount = 1;
        StartCheckingPath();
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
        DisableSlotsButtons();

        var startPipe = _pipeSlots.FirstOrDefault(slot => slot.IsStart);
        var endPipe = _pipeSlots.FirstOrDefault(slot => slot.IsEnd);

        if (startPipe.Type != _startConnectionRequired)
        {
            startPipe.Image.color = Color.red;
            yield break;
        }

        startPipe.Image.color = Color.green;
        var visited = new HashSet<PipeSlot>();

        yield return StartCoroutine(DFS(startPipe, endPipe, visited));
    }

    private IEnumerator DFS(PipeSlot current, PipeSlot end, HashSet<PipeSlot> visited)
    {
        visited.Add(current);

        yield return new WaitForSeconds(0.4f);

        if (current == end)
        {
            if (end.Type == _endConnectionRequired)
            {
                Debug.Log("Puzzle Completed!");
                _isCompleted = true;
                StartCoroutine(FillBotLiquid());
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

    private IEnumerator FillBotLiquid()
    {
        float elapsedTime = 0f;
        _botLiquid.fillAmount = 0;

        while (elapsedTime < 3)
        {
            _botLiquid.fillAmount = Mathf.Lerp(0, 1, elapsedTime / 3);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _botLiquid.fillAmount = 1;
    }

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

    private void Reset()
    {
        StopCoroutine(CheckPathStepByStepWithDFS());
        foreach (var pipeSlot in _pipeSlots)
        {
            pipeSlot.Reset();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymbolMatchPuzzleView : View
{
    [SerializeField] private SymbolSlot _symbolSlotPrefab;  // Prefab for slot
    [SerializeField] private Transform _slotsGrid;  // The grid layout parent
    [SerializeField] private Sprite[] _symbols;     // The symbol images

    private List<SymbolSlot> _slots;
    private SymbolSlot _firstSelected;
    private SymbolSlot _secondSelected;

    public static bool IsCheckingForMatch { get; private set; } = false;

    private int _matchCount = 0;

    private bool _isCompleted = false;

    public override void Hide()
    {
        base.Hide();

        if (_isCompleted)
        {
            return;
        }

        Reset();
    }

    public override void Initialize()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        _slots = new List<SymbolSlot>();
        var symbolList = new List<Sprite>();

        foreach (var symbol in _symbols)
        {
            symbolList.Add(symbol);
            symbolList.Add(symbol);
        }

        symbolList = symbolList.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < symbolList.Count; i++)
        {
            var newSlot = Instantiate(_symbolSlotPrefab, _slotsGrid);
            newSlot.Initialize(symbolList[i]);
            _slots.Add(newSlot);

            newSlot.OnSymbolSelected += OnSymbolSelected;
        }
    }

    private void OnSymbolSelected(SymbolSlot selectedSlot)
    {
        if (_firstSelected == null)
        {
            _firstSelected = selectedSlot;
        }
        else if (_secondSelected == null && _firstSelected != null)
        {
            _secondSelected = selectedSlot;
            IsCheckingForMatch = true;
            StartCoroutine(CheckForMatch());
        }
    }

    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1f);

        if (_firstSelected.GetSymbol() != _secondSelected.GetSymbol())
        {
            _firstSelected.HideSymbol();
            _secondSelected.HideSymbol();
        }
        else
        {
            _matchCount++;

            if (_matchCount == _symbols.Length)
            {
                _isCompleted = true;
                StaticEventsHandler.CallPuzzleCompletedEvent(this);
            }
        }

        _firstSelected = null;
        _secondSelected = null;
        IsCheckingForMatch = false;
    }

    private void Reset()
    {
        _matchCount = 0;
        _firstSelected = null;
        _secondSelected = null;
        IsCheckingForMatch = false;

        foreach (var slot in _slots)
        {
            slot.Reset();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class SymbolSlot : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private Image _image;
    private Sprite _symbol;
    private bool _isRevealed;

    public event Action<SymbolSlot> OnSymbolSelected;

    public void Initialize(Sprite symbol)
    {
        _symbol = symbol;
        HideSymbol();
    }

    public void Select()
    {
        if (SymbolMatchPuzzleView.IsCheckingForMatch)
        {
            return;
        }

        if (!_isRevealed)
        {
            ShowSymbol();
            OnSymbolSelected?.Invoke(this);
        }
    }

    public void ShowSymbol()
    {
        _image.sprite = _symbol;
        _isRevealed = true;
    }

    public void HideSymbol()
    {
        _image.sprite = null;
        _isRevealed = false;
    }

    public Sprite GetSymbol()
    {
        return _symbol;
    }

    public bool IsRevealed()
    {
        return _isRevealed;
    }
}

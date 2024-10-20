using DG.Tweening;
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
        _image.color = new Color(1, 1, 1, 0);
        _image.sprite = symbol;
        _isRevealed = false;
        _image.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void Select()
    {
        if (SymbolMatchPuzzleView.IsCheckingForMatch)
        {
            return;
        }

        if (!_isRevealed)
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.2f, RotateMode.LocalAxisAdd)
                .OnUpdate(() =>
                {
                    if (transform.eulerAngles.y >= 90 && transform.eulerAngles.y < 180)
                    {
                        ShowSymbol();
                    }
                })
                .OnComplete(() =>
                {
                    // Rotation complete, invoke the event
                    OnSymbolSelected?.Invoke(this);
                });
        }
    }

    public void ShowSymbol()
    {
        _image.color = new Color(1, 1, 1, 1);
        _isRevealed = true;
    }

    public void HideSymbol()
    {
        transform.DORotate(new Vector3(0, -180, 0), 0.2f, RotateMode.LocalAxisAdd)
                 .OnUpdate(() =>
                 {
                     // When we reach 90 degrees, hide the symbol (halfway point)
                     if (transform.eulerAngles.y >= 90 && transform.eulerAngles.y < 180)
                     {
                         _image.color = new Color(1, 1, 1, 0);
                     }
                 })
                 .OnComplete(() =>
                 {
                     // Rotation complete, mark the symbol as hidden
                     _isRevealed = false;
                 });
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

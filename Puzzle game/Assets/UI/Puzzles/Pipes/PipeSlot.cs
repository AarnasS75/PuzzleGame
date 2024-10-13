using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PipeSlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button _btn;
    [SerializeField] private Image _image;

    [Header("Configuration")]
    [SerializeField] private PipeType _type;
    [SerializeField] private bool _isStart;
    [SerializeField] private bool _isEnd;

    public PipeType Type => _type;
    public bool IsEnd => _isEnd;
    public bool IsStart => _isStart;
    public Image Image => _image;

    private bool _isRotating = false;

    public void Rotate()
    {
        if (_isRotating)
        {
            return;
        }

        _isRotating = true;

        float newZRotation = _image.rectTransform.eulerAngles.z + 90;

        _image.rectTransform.DORotate(new Vector3(0, 0, newZRotation), 0.2f, RotateMode.FastBeyond360)
             .SetEase(Ease.InOutQuad)
             .OnComplete(() =>
             {
                 _isRotating = false;
                 UpdatePipeTypeBasedOnRotation();
             });
    }

    public void Initialize(Sprite randomSprite, PipeType pipeType)
    {
        _image.sprite = randomSprite;
        int angle = Random.Range(0, 4) * 90;
        _image.rectTransform.rotation = Quaternion.Euler(0, 0, angle % 360);
        _type = pipeType;

        UpdatePipeTypeBasedOnRotation();
    }

    public bool IsConnectedCorrectly(PipeType nextPipeType)
    {
        bool isConnectionGood = false;

        switch (nextPipeType)
        {
            case PipeType.LeftToRight:
                isConnectionGood = _type == PipeType.LeftToRight || _type == PipeType.DownToLeft || _type == PipeType.UpToLeft;
                break;
            case PipeType.UpToDown:
                isConnectionGood = _type == PipeType.UpToLeft || _type == PipeType.UpToRight || _type == PipeType.UpToDown;
                break;
            case PipeType.DownToRight:
                isConnectionGood = _type == PipeType.UpToLeft || _type == PipeType.LeftToRight || _type == PipeType.DownToLeft;
                break;
            case PipeType.DownToLeft:
                isConnectionGood = _type == PipeType.UpToRight || _type == PipeType.DownToRight || _type == PipeType.LeftToRight;
                break;
            case PipeType.UpToRight:
                isConnectionGood = _type == PipeType.LeftToRight || _type == PipeType.DownToLeft || _type == PipeType.UpToLeft;
                break;
            case PipeType.UpToLeft:
                isConnectionGood = _type == PipeType.UpToDown || _type == PipeType.DownToRight || _type == PipeType.DownToLeft;
                break;
        }

        return isConnectionGood;
    }

    private void UpdatePipeTypeBasedOnRotation()
    {
        float currentRotationZ = _image.rectTransform.eulerAngles.z;

        currentRotationZ = Mathf.RoundToInt(currentRotationZ % 360);

        if (_type == PipeType.LeftToRight || _type == PipeType.UpToDown)
        {
            if (Mathf.Approximately(currentRotationZ, 0) || Mathf.Approximately(currentRotationZ, 180))
            {
                _type = PipeType.LeftToRight;
            }
            else if (Mathf.Approximately(currentRotationZ, 90) || Mathf.Approximately(currentRotationZ, 270))
            {
                _type = PipeType.UpToDown;
            }
        }
        else
        {
            if (Mathf.Approximately(currentRotationZ, 0))
            {
                _type = PipeType.UpToLeft;
            }
            else if (Mathf.Approximately(currentRotationZ, 90))
            {
                _type = PipeType.DownToLeft;
            }
            else if (Mathf.Approximately(currentRotationZ, 180))
            {
                _type = PipeType.DownToRight;
            }
            else if (Mathf.Approximately(currentRotationZ, 270))
            {
                _type = PipeType.UpToRight;
            }
        }
    }
}
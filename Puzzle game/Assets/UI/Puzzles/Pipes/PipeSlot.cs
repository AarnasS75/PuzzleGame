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

    public void Initialize(Sprite randomSprite)
    {
        _image.sprite = randomSprite;
        int angle = Random.Range(0, 4) * 90;
        _image.rectTransform.rotation = Quaternion.Euler(0, 0, angle % 360);

        UpdatePipeTypeBasedOnRotation();
    }

    public void SetType(PipeType type)
    {
        _type = type;
    }

    public bool IsConnectedCorrectly(PipeType nextPipeType, Vector2 direction)
    {
        bool isConnectionGood = false;

        switch (_type)
        {
            case PipeType.Horizontal:
                isConnectionGood =
                    (nextPipeType == PipeType.DownToRight && direction == Vector2.left) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.left) ||
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.left) ||
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.right) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.right) ||
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.right);
                break;

            case PipeType.Vertical:
                isConnectionGood =
                    (nextPipeType == PipeType.Vertical && direction == Vector2.up) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.up) ||
                    (nextPipeType == PipeType.DownToRight && direction == Vector2.up) ||
                    (nextPipeType == PipeType.Vertical && direction == Vector2.down) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.down) ||
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.down);
                break;

            case PipeType.DownToRight:
                isConnectionGood =
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.right) ||
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.right) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.right) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.down) ||
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.down) ||
                    (nextPipeType == PipeType.Vertical && direction == Vector2.down);
                break;

            case PipeType.DownToLeft:
                isConnectionGood =
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.left) ||
                    (nextPipeType == PipeType.DownToRight && direction == Vector2.left) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.left) ||
                    (nextPipeType == PipeType.Vertical && direction == Vector2.down) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.down) ||
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.down);
                break;

            case PipeType.UpToRight:
                isConnectionGood =
                    (nextPipeType == PipeType.UpToLeft && direction == Vector2.right) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.right) ||
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.right) ||
                    (nextPipeType == PipeType.Vertical && direction == Vector2.up) ||
                   (nextPipeType == PipeType.DownToLeft && direction == Vector2.up) ||
                    (nextPipeType == PipeType.DownToRight && direction == Vector2.up);
                break;

            case PipeType.UpToLeft:
                isConnectionGood =
                    (nextPipeType == PipeType.DownToRight && direction == Vector2.up) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.up) ||
                    (nextPipeType == PipeType.Vertical && direction == Vector2.up) ||
                    (nextPipeType == PipeType.Horizontal && direction == Vector2.left) ||
                    (nextPipeType == PipeType.UpToRight && direction == Vector2.left) ||
                    (nextPipeType == PipeType.DownToLeft && direction == Vector2.left);
                break;
        }

        return isConnectionGood;
    }

    private void UpdatePipeTypeBasedOnRotation()
    {
        float currentRotationZ = _image.rectTransform.eulerAngles.z;

        currentRotationZ = Mathf.RoundToInt(currentRotationZ % 360);

        if (_type == PipeType.Horizontal || _type == PipeType.Vertical)
        {
            if (Mathf.Approximately(currentRotationZ, 0) || Mathf.Approximately(currentRotationZ, 180))
            {
                _type = PipeType.Horizontal;
            }
            else if (Mathf.Approximately(currentRotationZ, 90) || Mathf.Approximately(currentRotationZ, 270))
            {
                _type = PipeType.Vertical;
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

    public void DisableButton()
    {
        _btn.interactable = false;
    }
}
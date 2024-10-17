using UnityEngine;
using UnityEngine.UI;

public class HudSlot : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void Initialize(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.color = new Color(1, 1, 1, 1);
    }

    public void ResetSlot()
    {
        _image.sprite = null;
        _image.color = new Color(1, 1, 1, 0);
    }
}
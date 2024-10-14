using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LightSlot : MonoBehaviour
{
    [SerializeField] private Image _image;

    private const float FLASH_DURATION = 0.15f;

    public event Action<LightSlot> OnSelected;

    public void Select()
    {
        Flash();
        OnSelected?.Invoke(this);
    }

    public void Flash()
    {
        StartCoroutine(nameof(FlashImage));
    }

    private IEnumerator FlashImage()
    {
        Color originalColor = _image.color;
        _image.color = Color.red;
        yield return new WaitForSeconds(FLASH_DURATION);
        _image.color = originalColor;
    }

    public void ResetColor()
    {
        _image.color = Color.white;
    }
}

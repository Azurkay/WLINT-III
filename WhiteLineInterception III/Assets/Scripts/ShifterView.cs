using System;
using UnityEngine;
using UnityEngine.UI;

public class ShifterView : MonoBehaviour
{
    [SerializeField] private Image _previousGearImage;
    [SerializeField] private Image _currentGearImage;
    [SerializeField] private Image _nextGearImage;

    public void ShifterUpdate(Sprite previousGearImage, Sprite currentGearImage, Sprite nextGearImage)
    {
        _previousGearImage.sprite = previousGearImage;
        _currentGearImage.sprite = currentGearImage;
        _nextGearImage.sprite = nextGearImage;
        if (previousGearImage == null)
        {
            _previousGearImage.color = new Color(0, 0, 0, 0);
        }
        else if (nextGearImage == null)
        {
            _nextGearImage.color = new Color(0, 0, 0, 0);
        }
    }
}

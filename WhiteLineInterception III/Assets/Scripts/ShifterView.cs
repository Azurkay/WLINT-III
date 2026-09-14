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
    }
}

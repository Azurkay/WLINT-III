using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShifterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _previousGearText;
    [SerializeField] private TextMeshProUGUI _currentGearText;
    [SerializeField] private TextMeshProUGUI _nextGearText;

    public void ShifterUpdate(string previousGearText, string currentGearText, string nextGearText)
    {
        _previousGearText.text = previousGearText;
        _currentGearText.text = currentGearText;
        _nextGearText.text = nextGearText;
    }
}

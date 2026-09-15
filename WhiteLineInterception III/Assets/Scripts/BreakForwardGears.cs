using System;
using TMPro;
using UnityEngine;


public class BreakForwardGears : MonoBehaviour
{

    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _timeToBreak = 5;
    [SerializeField] private TextMeshProUGUI _explanationGameObject;
    [SerializeField] private String _explanationText = "Oh the forward gears juste broked";
    [SerializeField] private float _timeToDesableExplanation;

    void Start()
    {
        _explanationGameObject.text = "";
        Destroy(gameObject, _timeToDesableExplanation);
    }


    void Update()
    {
        _timeToBreak -= Time.deltaTime;
        if (_timeToBreak <= 0)
        {
            _explanationGameObject.text = _explanationText;
            for (int i = 0; i < _camaroRef.GearsRatios.Length; i++)
            {
                if (_camaroRef.GearsRatios[i] > 0f)
                {
                    _camaroRef.GearsRatios[i] = 0f;
                }
            }
        }
    }
}

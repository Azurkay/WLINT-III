using System;
using TMPro;
using UnityEngine;


public class BreakForwardGears : MonoBehaviour
{

    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private TextMeshProUGUI _explanationGameObject;
    [SerializeField] private String _explanationText = "Oh the forward gears juste broked";
    [SerializeField] private float _timeToDesableExplanation;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        _explanationGameObject.text = _explanationText;
        _explanationGameObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
        _camaroRef.SlowDownCar(300);
        Destroy(_explanationGameObject, _timeToDesableExplanation);
        _explanationGameObject.gameObject.SetActive(true);
        for (int i = 0; i < _camaroRef.GearsRatios.Length; i++)
        {
            if (_camaroRef.GearsRatios[i] > 0f)
            {
                _camaroRef.GearsRatios[i] = 0f;
            }
         }
    }
}

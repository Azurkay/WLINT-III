using System;
using TMPro;
using UnityEngine;


public class BreakForwardGears : MonoBehaviour
{

    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _speedToSlowDown = 5;
    [SerializeField] private TextMeshProUGUI _explanationGameObject;
    [SerializeField] private String _explanationText = "Oh the forward gears juste broked";
    [SerializeField] private float _timeToDesableExplanation;
    [SerializeField] private AudioSource _audioSource;

    private bool _haveToSlowDown = false;

    void Start()
    {
        _explanationGameObject.text = _explanationText;
        _explanationGameObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
        _haveToSlowDown = true;
        Debug.Log(_haveToSlowDown);
        Destroy(_explanationGameObject, _timeToDesableExplanation);
        Destroy(gameObject, _timeToDesableExplanation + 2f);
        _explanationGameObject.gameObject.SetActive(true);
        for (int i = 0; i < _camaroRef.GearsRatios.Length; i++)
        {
            if (_camaroRef.GearsRatios[i] > 0f)
            {
                _camaroRef.GearsRatios[i] = 0f;
            }
         }
    }

    private void LateUpdate()
    {
        if (_haveToSlowDown == true)
        {
            _camaroRef.SlowDownCar(6000);
            Debug.Log("SlowDown");
            if (_camaroRef.RB.linearVelocity.magnitude * 3.6f < _speedToSlowDown)
            {
                _haveToSlowDown = false;
                Debug.Log("Stop to SlowDown");
            }
        }
    }
}

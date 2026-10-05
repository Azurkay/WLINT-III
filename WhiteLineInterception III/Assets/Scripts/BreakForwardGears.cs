using System;
using TMPro;
using UnityEngine;


public class BreakForwardGears : MonoBehaviour
{

    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _speedToSlowDown = 5;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _ohNON;

    private bool _haveToSlowDown = false;


    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
        _ohNON.SetActive(true);
        _haveToSlowDown = true;

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
            if (_camaroRef.RB.linearVelocity.magnitude * 3.6f < _speedToSlowDown)
            {
                _haveToSlowDown = false;
            }
        }
    }
}

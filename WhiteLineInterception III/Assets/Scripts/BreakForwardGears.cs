using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class BreakForwardGears : MonoBehaviour
{

    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _timeToBreak = 5;
    [SerializeField] private GameObject _explanationGameObject;
    [SerializeField] private float _timeToDesableExplanation;

    void Start()
    {
        Object.Destroy(_explanationGameObject, _timeToDesableExplanation);
    }


    void Update()
    {
        _timeToBreak -= Time.deltaTime;
        if (_timeToBreak <= 0)
        {
            _explanationGameObject.SetActive(true);
            for (int i = 0; i < _camaroRef.GearsRatios.Length; i++)
            {
                if (_camaroRef.GearsRatios[i] > 0f)
                {
                    _camaroRef.GearsRatios[i] = 0f;
                }
            }
            Object.Destroy(this, 5f);                   
        }
    }
}

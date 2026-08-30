using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] private float _timer = 15f;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _haveToStart = false;


    private void StartTimer()
    {
        _haveToStart = true;
    }

    private void StopTimer()
    {
        _haveToStart = false;
    }

    void Update()
    {
        if (_haveToStart == true && _timer > 0)
        {
            _timer -= Time.deltaTime;
            _text.text = _timer.ToString();
        }

        if (_timer <= 0)
        {
            Debug.Log("T'es mort mon reuf");
        }
    }
}

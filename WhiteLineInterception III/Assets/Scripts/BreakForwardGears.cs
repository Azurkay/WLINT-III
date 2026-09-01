using UnityEngine;
using UnityEngine.InputSystem;

public class BreakForwardGears : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _timeToBreak = 5;
    [SerializeField] private GameObject _explanationGameObject;
    [SerializeField] private float _timeToDesableExplanation;
    [SerializeField] private InputActionReference _inputActionReference;

    void Start()
    {
        Object.Destroy(this, _timeToBreak);
    }

    void OnDestroy()
    {
        Object.Destroy(_explanationGameObject);
    }

    void Update()
    {
        _timeToDesableExplanation -= Time.deltaTime;
        if (_timeToDesableExplanation <= 0)
        {
            _inputActionReference.action.ChangeBindingWithPath("W [Keyboard]");

            _explanationGameObject.SetActive(true);
        }
    }
}

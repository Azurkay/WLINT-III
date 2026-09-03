using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CamaroDrive : MonoBehaviour
{
    #region Serialized Attributes

    [SerializeField] private WheelCollider _FL;
    [SerializeField] private WheelCollider _FR;
    [SerializeField] private WheelCollider _RL;
    [SerializeField] private WheelCollider _RR;

    [SerializeField] private Transform _FLTransform;
    [SerializeField] private Transform _FRTransform;
    [SerializeField] private Transform _RLTransform;
    [SerializeField] private Transform _RRTransform;

    [SerializeField] private float _motorForce = 100f;
    [SerializeField] private float _steeringForce = 30f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _camaroCentreOfMass;


    #endregion

    #region Attributes
    private float _verticalInput;
    private float _horizontalInput;

    private float _maxVerticalInput = 1;
    private float _minVerticalInput = -1;

    #endregion

    #region Properties

    public float MaxVerticalInput
    {
        get => _maxVerticalInput;
        set => _maxVerticalInput = value;
    }

    public float MinVerticalInput
    {
        get => _minVerticalInput;
        set => _minVerticalInput = value;
    }

    #endregion

    private void MotorForce()
    {
        if (_RL.motorTorque > 0 && _verticalInput < 0)
        {
            _RL.motorTorque = _motorForce * _verticalInput * 100;
            _RR.motorTorque = _motorForce * _verticalInput * 100;
        } 
        else
        {
            _RL.motorTorque = _motorForce * _verticalInput;
            _RR.motorTorque = _motorForce * _verticalInput;
        }
    }

    private void SteeringWheels()
    {
        _FR.steerAngle = _steeringForce * _horizontalInput;
        _FL.steerAngle = _steeringForce * _horizontalInput;
    }

    private void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    private void UpdateWheel()
    {
        RotateWheel(_FL, _FLTransform);
        RotateWheel(_FR, _FRTransform);
        RotateWheel(_RL, _RLTransform);
        RotateWheel(_RR, _RRTransform);
    }

    private void GetInput()
    {
        _verticalInput = Mathf.Clamp(Input.GetAxis("Vertical"), MinVerticalInput, MaxVerticalInput);
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    #region Mono

    private void Start()
    {
        _rb.centerOfMass = _camaroCentreOfMass.localPosition;
    }

    private void Update()
    {
        GetInput();
        MotorForce();
        SteeringWheels();
        UpdateWheel();
    }

    #endregion
}

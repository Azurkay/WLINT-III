using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
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
    [SerializeField] private float _brakeForce = 300f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _camaroCentreOfMass;

    [SerializeField] private string[] _gearsNames = {"R",  "N",  "1",  "2",  "3"};
    [SerializeField] private float[] _gearsRatios = {-0.5f, 0f, 0.33f, 0.66f, 1f};
    [SerializeField] private int _currentGearIndex = 0;



    [SerializeField] private ShifterView _shifterView;
    [SerializeField] private SpeedView _speedView;
    [SerializeField] private float _realLifeWheelSize = 35.56f;



    #endregion

    #region Attributes
    private float _verticalInput;
    private float _horizontalInput;
    private float _gearsInput;

    private float _maxVerticalInput = 1;
    private float _minVerticalInput = -1;

    private float _currentSpeed = 0;
    private float _timeToUpdateSpeedMeter = 0.1f;
    private float _timeBeforeUpdateSpeedMeter = 0.1f;

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
        float motor = 0f;
        float brake = 0f;

        if (_verticalInput > 0f)
        {
            motor = _motorForce * _verticalInput * _gearsRatios[_currentGearIndex];
        }
        else if (_verticalInput < 0f)
        {
            brake = _brakeForce * -_verticalInput;
        }

        _RL.motorTorque = motor;
        _RR.motorTorque = motor;

        _FL.brakeTorque = brake;
        _FR.brakeTorque = brake;
        _RL.brakeTorque = brake;
        _RR.brakeTorque = brake;

        _timeBeforeUpdateSpeedMeter = _timeBeforeUpdateSpeedMeter - Time.deltaTime;

        if (_timeBeforeUpdateSpeedMeter <= 0)
        {
            CalculateSpeed();
            _timeBeforeUpdateSpeedMeter = _timeToUpdateSpeedMeter;
        }

    }

    private void SteeringWheels()
    {
        _FR.steerAngle = _steeringForce * _horizontalInput;
        _FL.steerAngle = _steeringForce * _horizontalInput;
    }

    private void ChangeGear()
    {

        if (Input.GetButtonDown("ShiftUp"))
        {
            _currentGearIndex++;
        }
        else if (Input.GetButtonDown("ShiftDown"))
        {
            _currentGearIndex--;
        }

        _currentGearIndex = Mathf.Clamp(_currentGearIndex, 0, _gearsRatios.Length - 1);

        string previousGearName = _currentGearIndex > 0 ? _gearsNames[_currentGearIndex - 1] : "";
        string currentGearName = _gearsNames[_currentGearIndex];
        string nextGearName = _currentGearIndex < _gearsNames.Length - 1 ? _gearsNames[_currentGearIndex + 1] : "";

        _shifterView.ShifterUpdate(previousGearName, currentGearName, nextGearName);

    }

    private void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    private void CalculateSpeed()
    {
        _speedView.UpdateSpeedView((int)(_rb.linearVelocity.magnitude * 3.6f));
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
        ChangeGear();
    }

    private void Update()
    {
        GetInput();
        ChangeGear();
        MotorForce();
        SteeringWheels();
        UpdateWheel();
    }

    #endregion
}

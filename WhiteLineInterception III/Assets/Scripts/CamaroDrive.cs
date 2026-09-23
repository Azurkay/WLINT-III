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
    [SerializeField] private float _brakeForce = 300f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _camaroCentreOfMass;

    [SerializeField] private Sprite[] _gearsImages;
    [SerializeField] private Sprite[] _gearsImagesAfterUnlock;
    [SerializeField] private float[] _gearsRatios = {-0.5f, 0f, 0.33f, 0.66f, 1f};
    [SerializeField] private float[] _gearsRatiosAfterUnlock = {4f, 1.5f, -0.5f, 0f, 0.33f, 0.66f, 1f};
    [SerializeField] private int _currentGearIndex = 0;

    [SerializeField] private float _nitroMotorForceRatio = 5;
    [SerializeField] private float _nitroDeltaTimeDecrementRatio = 2;
    [SerializeField] private float _nitroDeltaTimeIncrementRatio = 1;
    [SerializeField] private float _maxNitroAmound = 1200;
    [SerializeField] private NitroView _nitroView;



    [SerializeField] private ShifterView _shifterView;
    [SerializeField] private SpeedView _speedView;
    [SerializeField] private float _realLifeWheelSize = 35.56f;

    
    [SerializeField] private AudioSource _motorSound;
    [SerializeField] private AudioSource _motorEffectSound;
    [SerializeField] private AudioClip _engineStart;
    [SerializeField] private AudioClip _forwardGearsExplode;
    [SerializeField] private AudioClip _engine;
    [SerializeField] private AudioClip _engineIdle;

    #endregion

    #region Attributes
    private float _verticalInput;
    private float _horizontalInput;

    private float _timeToUpdateSpeedMeter = 0.1f;
    private float _timeBeforeUpdateSpeedMeter = 0.1f;

    private float _currentNitroAmound;
    private bool _nitroUnlock = false;

    #endregion

    #region Properties

    public float[] GearsRatios
    {
        get => _gearsRatios;
        set => _gearsRatios = value;
    }

    public bool NitroUnlock
    {
        get => _nitroUnlock;
        set => _nitroUnlock = value;
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

    public void SlowDownCar(float brakeForce)
    {
        _FL.brakeTorque = brakeForce;
        _FR.brakeTorque = brakeForce;
        _RL.brakeTorque = brakeForce;
        _RR.brakeTorque = brakeForce;
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

        Sprite previousGearImage = _currentGearIndex > 0 ? _gearsImages[_currentGearIndex - 1] : null;
        Sprite currentGearImage = _gearsImages[_currentGearIndex];
        Sprite nextGearImage = _currentGearIndex < _gearsImages.Length - 1 ? _gearsImages[_currentGearIndex + 1]: null;

        _shifterView.ShifterUpdate(previousGearImage, currentGearImage, nextGearImage);
    }

    private void Nitro()
    {
        if (NitroUnlock == true)
        {
            bool isHeld = Input.GetButton("Nitro") && _currentNitroAmound > 0f;

            if (Input.GetButtonDown("Nitro"))
            {
                if (_currentNitroAmound > 0)
                {
                    _motorForce *= _nitroDeltaTimeDecrementRatio;   
                }
                else
                {
                    _motorForce /= _nitroDeltaTimeDecrementRatio;                
                }
            }
            else if (Input.GetButtonUp("Nitro"))
            {
                _motorForce /= _nitroDeltaTimeDecrementRatio;
            }
            if (isHeld)
            {
                _currentNitroAmound = Mathf.Clamp(_currentNitroAmound - Time.deltaTime * _nitroDeltaTimeDecrementRatio, 0, _maxNitroAmound);
            }
            else
            {
                _currentNitroAmound = Mathf.Clamp(_currentNitroAmound + Time.deltaTime * _nitroDeltaTimeIncrementRatio, 0, _maxNitroAmound);
            }

            _nitroView.UpdateNitro(_currentNitroAmound, _maxNitroAmound);
        }
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
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    public void UnlockBackwardGears()
    {
        _gearsImages = _gearsImagesAfterUnlock;
        _gearsRatios = _gearsRatiosAfterUnlock;
    }

    #region Mono

    private void Start()
    {
        _currentNitroAmound = _maxNitroAmound;
        _rb.centerOfMass = _camaroCentreOfMass.localPosition;
        ChangeGear();
    }

    private void Update()
    {
        GetInput();
        ChangeGear();
        Nitro();
        MotorForce();
        SteeringWheels();
        UpdateWheel();
    }

    #endregion
}

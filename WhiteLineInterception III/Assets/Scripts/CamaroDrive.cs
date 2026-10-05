using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CamaroDrive : MonoBehaviour
{
    #region Serialized Attributes

    [SerializeField] private WheelCollider _fL;
    [SerializeField] private WheelCollider _fR;
    [SerializeField] private WheelCollider _rL;
    [SerializeField] private WheelCollider _rR;

    [SerializeField] private Transform _fLTransform;
    [SerializeField] private Transform _fRTransform;
    [SerializeField] private Transform _rLTransform;
    [SerializeField] private Transform _rRTransform;

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
    [SerializeField] private AudioClip _nitroSound;
    [SerializeField] private AudioClip _unlockBackwardGearsSound;

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

    public Rigidbody RB
    {
        get => _rb;
        set => _rb = value;
    }

    public AudioSource MotorEffectSound
    {
        get => _motorEffectSound;
        set => _motorEffectSound = value;
    }

    #endregion

    private void MotorForceCarInput()
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

        _rL.motorTorque = motor;
        _rR.motorTorque = motor;

        _fL.brakeTorque = brake;
        _fR.brakeTorque = brake;
        _fL.brakeTorque = brake;
        _fR.brakeTorque = brake;

        _timeBeforeUpdateSpeedMeter = _timeBeforeUpdateSpeedMeter - Time.deltaTime;

        if (_timeBeforeUpdateSpeedMeter <= 0)
        {
            CalculateSpeed();
            _timeBeforeUpdateSpeedMeter = _timeToUpdateSpeedMeter;
        }

    }

    private void MotorSound()
    {
        _motorSound.pitch = 1 + ((_rb.linearVelocity.magnitude * 3.6f) / 100);
    }

    public void SlowDownCar(float brakeForce)
    {
        _fL.brakeTorque = brakeForce;
        _fR.brakeTorque = brakeForce;
        _rL.brakeTorque = brakeForce;
        _rR.brakeTorque = brakeForce;
    }

    private void SteeringWheels()
    {
        _fR.steerAngle = _steeringForce * _horizontalInput;
        _fL.steerAngle = _steeringForce * _horizontalInput;
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

    public void SetActiveNitroView()
    {
        _nitroView.gameObject.SetActive(true);
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
                //_rb.linearVelocity = transform.forward * 200;
                // Truc stylé à faire ici
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
        RotateWheel(_fL, _fLTransform);
        RotateWheel(_fR, _fRTransform);
        RotateWheel(_rL, _rLTransform);
        RotateWheel(_rR, _rRTransform);
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

    private void ResetCarPosition()
    {
        if (Input.GetAxis("Reset") > 0.1f)
        {
            float rotationY = transform.eulerAngles.y + 45f;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
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
        MotorForceCarInput();
        SteeringWheels();
        UpdateWheel();
        MotorSound();
        ResetCarPosition();
    }

    #endregion
}

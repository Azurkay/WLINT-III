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

    private float verticalInput;
    private float horizontalInput;

    #endregion

    private void MotorForce()
    {
        _RL.motorTorque = _motorForce * verticalInput * -1;
        _RR.motorTorque = _motorForce * verticalInput * -1;
    }

    private void SteeringWheels()
    {
        _FR.steerAngle = _steeringForce * horizontalInput;
        _FL.steerAngle = _steeringForce * horizontalInput;
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
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }




    #region Mono

    private void Start()
    {
        _rb.centerOfMass = _camaroCentreOfMass.localPosition;
    }

    private void Update()
    {
        MotorForce();
        SteeringWheels();
        UpdateWheel();
        GetInput();
    }

    #endregion
}

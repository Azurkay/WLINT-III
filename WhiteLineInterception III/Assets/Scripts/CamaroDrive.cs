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

    #endregion

    #region Attributes
        
    #endregion

    private void MoveForward()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            _FL.motorTorque = 100f;
            RotateWheel(_FL, _FLTransform);
            _FR.motorTorque = 100f;
            RotateWheel(_FR, _FRTransform);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            _FL.motorTorque = 0;
            _FR.motorTorque = 0;
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





    #region Mono

    private void Update()
    {
        MoveForward();
    }

    #endregion
}

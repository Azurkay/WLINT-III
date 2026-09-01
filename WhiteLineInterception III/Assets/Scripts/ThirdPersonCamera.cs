using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private float _sensibility = 1;
    [SerializeField] private float _rotationMax = 80f;
    [SerializeField] private float _rotationMin = -80f;

    private float _rotationY = 0;
    private float _rotationX = 0;


    public float Sensibility
    {
        get => _sensibility;
        set => _sensibility = value;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Sensibility;
        float mouseY = Input.GetAxis("Mouse Y") * Sensibility;

        _rotationY += mouseX;
        _rotationX -= mouseY;

        _rotationX = Mathf.Clamp(_rotationX, _rotationMin, _rotationMax);

        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class DestructionDetector : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _brakeForce = 10000;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
        rb.mass = 1f;
        Debug.Log(rb.mass);
        _camaroRef.SlowDownCar(_brakeForce);
    }
}

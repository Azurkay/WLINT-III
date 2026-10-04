using Unity.VisualScripting;
using UnityEngine;

public class DestructionDetector : MonoBehaviour
{
    [SerializeField] private float _brakeForce = 10000;
    [SerializeField] private CamaroDrive _camaroRef;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        _camaroRef.SlowDownCar(7000);
    }
}

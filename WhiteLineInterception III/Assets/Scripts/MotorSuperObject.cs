using UnityEngine;

public class MotorSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.UnlockBackwardGears();
    }
}

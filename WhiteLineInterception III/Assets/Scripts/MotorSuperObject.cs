using UnityEngine;

public class MotorSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private AudioSource _audioSource;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.UnlockBackwardGears();
        _audioSource.Play();
    }
}

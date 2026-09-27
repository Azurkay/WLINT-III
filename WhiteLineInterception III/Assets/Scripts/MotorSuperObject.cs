using UnityEngine;

public class MotorSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private AudioClip _audioClip;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.UnlockBackwardGears();
        _camaroRef.MotorEffectSound.clip = _audioClip;
        _camaroRef.MotorEffectSound.Play();
        Destroy(gameObject);
    }
}

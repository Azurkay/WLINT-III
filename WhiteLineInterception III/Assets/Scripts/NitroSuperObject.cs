using UnityEngine;

public class NitroSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private AudioClip _audioClip;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.NitroUnlock = true;
        _camaroRef.MotorEffectSound.clip = _audioClip;
        _camaroRef.MotorEffectSound.Play();
        Destroy(gameObject);
    }
}

using UnityEngine;

public class NitroSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private AudioSource _audioSource;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.NitroUnlock = true;
        _audioSource.Play();
    }
}

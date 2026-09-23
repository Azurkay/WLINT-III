using UnityEngine;

public class NitroSuperObject : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    void OnTriggerEnter(Collider other)
    {
        _camaroRef.NitroUnlock = true;
    }
}

using UnityEngine;

public class BreakForwardGears : MonoBehaviour
{
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private float _timeToBreak = 5;

    void Start()
    {
        Object.Destroy(this, _timeToBreak);
    }

    void OnDestroy()
    {
        
    }
}

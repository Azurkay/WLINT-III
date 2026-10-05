using UnityEngine;

public class ZiakDetector : MonoBehaviour
{
    [SerializeField] private GameObject _ZiakCreator;
    [SerializeField] private GameObject _Ziak;
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private GameObject _camaroCamera;

    void OnTriggerEnter(Collider other)
    {
        _ZiakCreator.SetActive(true);
        _camaroCamera.SetActive(false);
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        _camaroRef.RB.linearVelocity = Vector3.zero;
        Destroy(gameObject, 0.5f);
    }

    public void RestartGame()
    {
        _ZiakCreator.SetActive(false);
        _Ziak.SetActive(true);
        _camaroCamera.SetActive(true);
    }


}

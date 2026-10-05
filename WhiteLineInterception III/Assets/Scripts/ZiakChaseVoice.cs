using UnityEngine;

public class ZiakChaseVoice : MonoBehaviour
{

    [SerializeField] private GameObject _voice;
    void OnTriggerEnter(Collider other)
    {
        _voice.SetActive(true);
    }
}

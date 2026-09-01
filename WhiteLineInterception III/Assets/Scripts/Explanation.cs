using UnityEngine;

public class Explanation : MonoBehaviour
{
    [SerializeField] private float _timerToBegin = 10f;

    void Start()
    {
        Object.Destroy(transform.gameObject, _timerToBegin);
    }
}

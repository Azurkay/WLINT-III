using UnityEngine;

public class JokeEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _endingText;
    [SerializeField] private bool _isTheLast = false;
    void Update()
    {
        transform.position += Vector3.left * 5;
    }

    private void OnDestroy()
    {
        if (_isTheLast == true)
        {
            _endingText.SetActive(true);
        }
    }
}

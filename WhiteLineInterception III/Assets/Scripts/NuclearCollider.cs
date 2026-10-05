using UnityEngine;

public class NuclearCollider : MonoBehaviour
{
    [SerializeField] private GameObject _camaro;
    [SerializeField] private Canvas _deathMenu;
    [SerializeField] private Canvas _gameUI;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _camaro)
        {
            _deathMenu.gameObject.SetActive(true);
            _gameUI.gameObject.SetActive(false);
        }
    }
}

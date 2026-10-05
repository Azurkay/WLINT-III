using UnityEngine;

public class SetActiveGameObject : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private bool _isActive;

    public void SetIsActiveGameObject()
    {
        _gameObject.SetActive(_isActive);
    }
}

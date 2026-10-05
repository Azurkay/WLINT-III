using UnityEngine;

public class ZiakDetector : MonoBehaviour
{
    [SerializeField] private GameObject _ziakCreator;
    [SerializeField] private GameObject _ziak;
    [SerializeField] private GameObject _ziakMusic;
    [SerializeField] private CamaroDrive _camaroRef;
    [SerializeField] private GameObject _camaroCamera;
    [SerializeField] private GameObject _gameUI;


    private bool _alreadyEnter = false;
    
    void OnTriggerEnter(Collider other)
    {
        _ziakCreator.SetActive(true);
        _ziakMusic.SetActive(true);
        _camaroCamera.SetActive(false);
        _alreadyEnter = true;
        _gameUI.SetActive(false);
        Destroy(gameObject, 5f);
    }

    public void RestartGame()
    {
        _ziakCreator.SetActive(false);
        _ziak.SetActive(true);
        _camaroCamera.SetActive(true);
        _gameUI.SetActive(true);
    }

    void Update()
    {
        if (_alreadyEnter)
        {
            _camaroRef.RB.linearVelocity = Vector3.zero;
        }
    }


}

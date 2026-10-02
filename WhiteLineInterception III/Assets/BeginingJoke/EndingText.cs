using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingText : MonoBehaviour
{

    [SerializeField] private float _time;
    [SerializeField] private string _mainMenuLevel;
    void Start()
    {
        Destroy(this, 6f);
    }

    void OnDestroy()
    {
        SceneManager.LoadScene(_mainMenuLevel);
    }
}

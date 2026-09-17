using UnityEngine;

public class EditorOpti : MonoBehaviour
{
    [SerializeField] private GameObject[] _decorParents;


    void Start()
    {
        foreach (GameObject currentDecor in _decorParents)
        {
            currentDecor.SetActive(true);
        }
    }

}

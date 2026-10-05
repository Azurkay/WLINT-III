using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BunkerArrival : MonoBehaviour
{
    [SerializeField] private String _endingLevel;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(_endingLevel);
    }
}

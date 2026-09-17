using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{

    [SerializeField] private String _gameLevel;

    public void OpenGameLevel()
    {
        SceneManager.LoadScene(_gameLevel);
    }

}

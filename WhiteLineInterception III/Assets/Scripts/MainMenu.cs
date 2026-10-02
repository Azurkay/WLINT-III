using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private String _gameLevel;
    [SerializeField] private GameObject _fadeOut;
    [SerializeField] private String _settingsLevel;


    public String GameLevel
    {
        get { return _gameLevel; }
    }

    public String SettingsLevel
    {
        get { return _settingsLevel; }
    }

    public void OpenGameLevel()
    {
        _fadeOut.SetActive(true);
    }

    public void OpenSettingsLevel()
    {
        SceneManager.LoadScene(SettingsLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}

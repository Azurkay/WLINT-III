using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private String _gameLevel;
    [SerializeField] private Animation _cameraAnimation;
    [SerializeField] private Animation[] _exitAnimation;
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
        foreach (Animation animation in _exitAnimation)
        {
            animation.Play();
            Debug.Log(animation.name);
        }
        _cameraAnimation.Play();
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

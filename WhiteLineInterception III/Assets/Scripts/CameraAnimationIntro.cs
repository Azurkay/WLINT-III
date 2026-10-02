using UnityEngine;

public class CameraAnimationIntro : MonoBehaviour
{
    [SerializeField] private GameObject _canva;
    [SerializeField] private GameObject _1;
    [SerializeField] private GameObject _2;
    [SerializeField] private GameObject _3;
    [SerializeField] private GameObject _4;
    [SerializeField] private GameObject _wTI;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _quitButton;

    public void SetActiveCanva()
    {
        _canva.SetActive(true);
    }
    public void SetActive1()
    {
        _1.SetActive(true);
    }
    public void SetActive2()
    {
        _2.SetActive(true);
    }
    public void SetActive3()
    {
        _3.SetActive(true);
    }
    public void SetActive4()
    {
        _4.SetActive(true);
    }

    public void SetActiveWTI()
    {
        _wTI.SetActive(true);
    }

    public void SetActivePlayButton()
    {
        _playButton.SetActive(true);
    }
    public void SetActiveSettingsButton()
    {
        _settingsButton.SetActive(true);
    }
    public void SetActiveQuitButton()
    {
        _quitButton.SetActive(true);
    }

}

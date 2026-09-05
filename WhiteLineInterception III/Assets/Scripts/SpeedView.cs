using TMPro;
using UnityEngine;

public class SpeedView : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _speedText;
    public void UpdateSpeedView(int speed)
    {
        _speedText.text = speed.ToString();
    }

}

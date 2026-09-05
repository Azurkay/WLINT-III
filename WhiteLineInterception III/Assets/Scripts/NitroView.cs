using UnityEngine;
using UnityEngine.UI;

public class NitroView : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollBar;
    public void UpdateNitro(float currentValue, float maxValue)
    {
        _scrollBar.size = currentValue / maxValue;
    }


}

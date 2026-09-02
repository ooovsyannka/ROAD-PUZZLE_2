using TMPro;
using UnityEngine;

public class TimerRender : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer;

    public void UpdateTimer(int minute , float second)
    {
        _timer.text = $"{minute:D2}:{Mathf.FloorToInt(second):D2}";
    }

}
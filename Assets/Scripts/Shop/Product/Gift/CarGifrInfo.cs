using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class CarGifrInfo : CarInfo
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _procentage;

    public void UpdateSlider(float completedProcent)
    {
        _slider.value = completedProcent;
        _procentage.text = completedProcent.ToString();
    }
}
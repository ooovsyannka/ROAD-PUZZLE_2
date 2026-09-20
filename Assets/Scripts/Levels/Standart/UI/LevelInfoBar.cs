using TMPro;
using UnityEngine;

public class LevelInfoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelNumberText;

    public void UpdateLevelNumber(int levelNumber)
    {
        _levelNumberText.text = levelNumber.ToString();
    }
}
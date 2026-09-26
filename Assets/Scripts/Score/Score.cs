using TMPro;
using UnityEngine;

public class Score: Window
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    public  TextMeshProUGUI ScoreText => _scoreText;
}
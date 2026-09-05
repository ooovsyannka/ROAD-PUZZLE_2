using TMPro;
using UnityEngine;

public class BestScoreRender : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private BestScoreSaver _bestScoreSaver;

    public void ShowBestScore()
    {
        _bestScore.text = _bestScoreSaver.LoadBestScore().ToString();
    }
}

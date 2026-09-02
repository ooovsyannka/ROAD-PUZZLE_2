using UnityEngine;
using TMPro;

public class MenuRender : MonoBehaviour
{
    [SerializeField] private LevelInfoBar _levelInfoBar;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _messageAllLevelComplete;
    [SerializeField] private BestScoreRender _bestScoreRender;

    private void OnEnable()
    {
        _bestScoreRender.ShowBestScore();
    }

    public void ShowLevelNumber(int levelNumber)
    {
        if (levelNumber > 0)
        {
            _levelNumberText.text = levelNumber.ToString();
            _messageAllLevelComplete.gameObject.SetActive(false);
        }
    }

    public void ShowMessage()
    {
        _messageAllLevelComplete.gameObject.SetActive(true);
        _levelInfoBar.gameObject.SetActive(false);
    }
}

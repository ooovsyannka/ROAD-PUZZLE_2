using TMPro;
using UnityEngine;

public class EndGameScreen : Window
{
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private AddCoinButton _addCoinButton;
    [SerializeField] private TextMeshProUGUI _finishGameText;
    [SerializeField] private HomeButton _homeButton;

    private void OnEnable()
    {
        Close();
    }

    public void SetLevelMode(LevelMode levelMode, LevelData levelData = null)
    {
        _restartButton.SetLevelMode(levelMode, levelData);
    }

    public override void Close()
    {
        base.Close();
        _restartButton.gameObject.SetActive(false);
        _homeButton.gameObject.SetActive(false);
        _addCoinButton.gameObject.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        _restartButton.gameObject.SetActive(true);
        _homeButton.gameObject.SetActive(true);
        _addCoinButton.gameObject.SetActive(true);
    }

    public void ShowLoosInfo(string textFinishGame)
    {
        _finishGameText.text = textFinishGame;
    }
}

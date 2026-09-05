using System.Collections;
using TMPro;
using UnityEngine;

public class InfinityScoreRender : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private TextMeshProUGUI _earnedCoinText;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private float _durationUpdateScore;
    [SerializeField] private float _speedUpdateScore;

    private Coroutine _smothlyUpdateScore;
    private int _currentScore = 0;

    public void UpdateScore(int targetCount)
    {
        StartCoroutine(SmothlyUpdateScore(_currentScore, targetCount, _scoreText));
        _currentScore = targetCount;
    }

    public void UpdateBestScore( int targetCount)
    {
        StartCoroutine(SmothlyUpdateScore(0, targetCount, _scoreText));
    }

    public void ShowTotalScore(int targetCount)
    {
        StartCoroutine(SmothlyUpdateScore(0, targetCount, _totalScoreText));
    }

    public void ShowBestScore(int bestScore)
    {
        _bestScore.text = bestScore.ToString();
    }

    public void ShowEarnedCoinCount(int coinCount)
    {
        StartCoroutine(SmothlyUpdateScore(0, coinCount, _earnedCoinText));
    }

    private IEnumerator SmothlyUpdateScore(int startScore, int targetScore, TextMeshProUGUI scoreText)
    {
        float time = 0;
        int countToText;

        while (time < _durationUpdateScore)
        {
            time += Time.deltaTime;
            float progress = Mathf.Clamp01(time / _durationUpdateScore);
            countToText = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, progress));
            scoreText.text = countToText.ToString();

            yield return null;
        }

        countToText = targetScore;
        scoreText.text = countToText.ToString();
    }

    private void LaunchCoroutine(int startCount, int targetCount, TextMeshProUGUI text)
    {
        if (_smothlyUpdateScore != null)
            StopCoroutine(_smothlyUpdateScore);

        _smothlyUpdateScore = StartCoroutine(SmothlyUpdateScore(startCount, targetCount, text));
    }
}
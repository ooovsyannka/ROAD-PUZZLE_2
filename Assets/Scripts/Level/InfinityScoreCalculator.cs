using System.Collections.Generic;
using UnityEngine;

public class InfinityScoreCalculator : MonoBehaviour
{
    [SerializeField] private Chains _chains;
    [SerializeField] private InfinityScoreRender _infinityScoreRender;
    [SerializeField] private BestScore _bestScoreBar;
    [SerializeField] private ComboWindow _comboWindow;
    [SerializeField] private int _score;

    private int _bestScore;
    private int _scoreMultiple = 10;
    private int _minRoadCountForCombo = 3;
    private float _finalScoreMultiplier = 0.1f;
    private float _comboMultiple = 0.5f;
    private int _earnedCoin;

    public  int EarnedCoin => _earnedCoin;
    public int Score => _score;

    private void OnEnable()
    {
        _chains.ChainCompleted += ChainCalculateScore;
        _bestScoreBar.BestScoreRender.ShowBestScore();
    }

    public void ShowTotalResult(int collectedСoins)
    {
        _earnedCoin = (_score * _finalScoreMultiplier).RoundIntToNearest() + collectedСoins;
        _bestScoreBar.BestScoreRender.gameObject.SetActive(false);
        _infinityScoreRender.ShowTotalScore(_score);
        _infinityScoreRender.ShowBestScore(_score);
        _infinityScoreRender.ShowEarnedCoinCount(_earnedCoin);
        TryUpdateBestScore(_score);
    }

    public void ChainCalculateScore(List<Road> roads)
    {
        float score = 0;
        float comboMultiple = 1;
        int roadCount = 0;

        foreach (Road road in roads)
        {
            foreach (SingleRoad singleRoad in road.SingleRoadHolder.SingleRoads)
            {
                if (roadCount > _minRoadCountForCombo)
                {
                    comboMultiple += _comboMultiple;
                }

                score += _scoreMultiple * comboMultiple;
                roadCount++;
            }
        }

        if(roadCount >  _minRoadCountForCombo)
        {
            _comboWindow.Open();
            _comboWindow.SetComboCount(roadCount);
        }

        CalculateScore(score.RoundIntToNearest());
    }

    private void CalculateScore(int scoreCount)
    {
        _score += scoreCount;
        _infinityScoreRender.UpdateScore(_score);
    }

    private void TryUpdateBestScore(int currentCount)
    {
        if (_bestScore < currentCount)
        {
            _infinityScoreRender.UpdateBestScore(currentCount);
            _bestScoreBar.BestScoreSaver.SaveBestScore(currentCount);
        }
        else
        {
            _infinityScoreRender.UpdateBestScore(_bestScore);
        }
    }
}
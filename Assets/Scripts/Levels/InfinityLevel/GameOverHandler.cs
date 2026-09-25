using System;
using System.Collections;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private Chains _chains;
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private GiveUpButton _giveUpButton;
    [SerializeField] private InfinityScoreCalculator _infinityScoreCalculator;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Grid _grid;
    [SerializeField] private WalletSaver _walletSaver;
    [SerializeField] private Timer _timer;
    [SerializeField] private BonusCollectionHandler  _bonusCollectionHandler;

    private void OnEnable()
    {
        _roadDrager.DragOver += TryFinidhGame;
        _giveUpButton.OnGiveUp += FinishGame;
        _timer.TimeIsOvered += FinishGame;
    }

    private void OnDisable()
    {
        _roadDrager.DragOver -= TryFinidhGame;
        _giveUpButton.OnGiveUp -= FinishGame;
        _timer.TimeIsOvered -= FinishGame;
    }

    private void Start()
    {
        _endGameScreen.SetLevelMode(LevelMode.Infinity);
    }

    private void TryFinidhGame(RoadNode _)
    {
        if (_chains.ChainCopmete)
            return;

        if (_grid.IsFull == false)
        {
            FinishGame("Закончилось Место");
        }
    }

    private void FinishGame(string argumet)
    {
        if (_chains.ChainCopmete == false)
        {
            _endGameScreen.Open();
            _endGameScreen.ShowLoosInfo(argumet);
            _infinityScoreCalculator.ShowTotalResult(_bonusCollectionHandler.CollectedCoins);
            _walletSaver.AddCoinInSave(_infinityScoreCalculator.EarnedCoin);
            _inputReader.StopReadInput();
        }
        else
        {
            StartCoroutine(WaitToChainComplete(argumet));
        }
    }

    private IEnumerator WaitToChainComplete(string argumet)
    {
        while (_chains.ChainCopmete)
        {
            yield return null;
        }

        if (_timer)
            FinishGame(argumet);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityLevel : MonoBehaviour
{
    [SerializeField] private List<UniversalRoadBoundary> _universalRoadNodes;
    [SerializeField] private Chains _chains;
    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private LightCycle _lightCycle;
    [SerializeField] private RoadBuilder _roadBuilder;
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Timer _timer;
    [SerializeField] private InfinityScoreCalculator _infinityScoreCalculator;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private HomeButton _homeButton;
    [SerializeField] private WalletSaver _walletSaver;
    [SerializeField] private BonusSpawner _bonusSpawner;
    [SerializeField] private GiveUpButton _giveUpButton;

    private int _startLevelTime = 3;
    private int _collectedСoins = 0;
    private Grid _grid;

    private IStartRoad _currentStartRodNode;
    private UniversalRoadBoundary _currentUniversalStartRoad;
    private UniversalRoadBoundary _brokeUniversalRoad;

    private void OnEnable()
    {
        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            if (universalRoadNode is IFinishRoad universalFinishRoad)
            {
                universalFinishRoad.OnConnected += TryCompleteChain;
            }
        }

        _timer.TimeIsOvered += FinishGame;
        _roadDrager.DragOver += TryFinidhGame;
        _giveUpButton.OnGiveUp += FinishGame;

        _timer.SetMaxMinute(_startLevelTime);
        _timer.LaunchCountdown();
        _roadBuilder.SetUniversalRoadNode(_universalRoadNodes);
    }

    private void OnDisable()
    {
        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            if (universalRoadNode is IFinishRoad universalFinishRoad)
            {
                universalFinishRoad.OnConnected -= TryCompleteChain;
            }
        }

        _timer.TimeIsOvered -= FinishGame;
        _roadDrager.DragOver -= TryFinidhGame;
        _giveUpButton.OnGiveUp -= FinishGame;
    }

    private void Start()
    {
        _grid.InitializeGrid();
        _endGameScreen.SetLevelMode(LevelMode.Infinity);

        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            if (_grid.TryGetCell(universalRoadNode.transform.position, out Cell cell))
            {
                cell.SetRoadNode(universalRoadNode);
                _chains.CreateChain(universalRoadNode);
            }
        }

        NextMove();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            print("1");
            //TrySpawnWatch();
        }
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    private void NextMove()
    {
        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            universalRoadNode.SetFinishType();
        }

        _currentUniversalStartRoad = _universalRoadNodes[Random.Range(0, _universalRoadNodes.Count)];
        Car car = _carSpawner.GetRandomCar(_currentUniversalStartRoad.transform.position + Vector3.up);
        car.UpdateHeadlightsBasedOnTime(_lightCycle.TimeOfDay);
        _currentUniversalStartRoad.SetStarType();
        _currentUniversalStartRoad.SetCar(car);
        _currentStartRodNode = _currentUniversalStartRoad;
        car.transform.rotation = _currentUniversalStartRoad.transform.rotation;

        TryBrokeUniversalFinishRoad();

        AttemptSpawnBonus();
    }

    private void AttemptSpawnBonus()
    {
        if (_bonusSpawner.TrySpawnClock(_grid.TryGetRandomEmptyCell().transform.position, out Clock clock))
        {
            clock.OnCollected += CollectClock;
        }
        else if (_bonusSpawner.TrySpawnCoin(_grid.TryGetRandomEmptyCell().transform.position, out Coin coin))
        {
            coin.OnCollected += CollectCoin;
        }
    }

    private void TryCompleteChain(IFinishRoad iFinishRoad)
    {
        if (iFinishRoad is UniversalRoadBoundary universalFinishRoad)
        {
            Car car = _currentUniversalStartRoad.Car;
            Route route = _chains.CreateRoute(_currentStartRodNode, universalFinishRoad.SplineComputer);
            car.Move(route.SplineComputer, universalFinishRoad.SplineComputer);
            car.MoveFinished += CleanChain;
            car.MoveFinished += route.CleanSplineComputer;
        }
    }

    private void CleanChain(Car car)
    {
        _chains.CleanChain(_currentUniversalStartRoad, _grid);
        car.MoveFinished -= CleanChain;
        _lightCycle.TryChangeLightIntensity();

        NextMove();
        car.Die();
    }

    private void TryFinidhGame(RoadNode _)
    {
        if (_chains.ChainCopmete == false)
        {
            if (_grid.IsFull == false)
            {
                FinishGame("Закончилось Место");
            }
        }
    }

    private void FinishGame(string argumet)
    {
        if (_chains.ChainCopmete == false)
        {
            _endGameScreen.Open();
            _endGameScreen.ShowLoosInfo(argumet);
            _infinityScoreCalculator.ShowTotalResult(_collectedСoins);
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

        if(_timer)
        FinishGame(argumet);
    }

    private void TryBrokeUniversalFinishRoad()
    {
        float i = 0.4f;

        if (_brokeUniversalRoad != null)
        {
            _brokeUniversalRoad.Fix();
        }

        if (Random.Range(0, 1f) > i)
        {
            UniversalRoadBoundary randomUniversalRoadBoundary = _universalRoadNodes[Random.Range(0, _universalRoadNodes.Count)];

            if (randomUniversalRoadBoundary.RoadBoundaryType == RoadBoundaryType.Finish)
            {
                randomUniversalRoadBoundary.Broken();
                _brokeUniversalRoad = randomUniversalRoadBoundary;
            }
        }
    }

    private void CollectCoin(Coin coin, int countCoin)
    {
        coin.OnCollected -= CollectCoin;
        _collectedСoins += countCoin;
    }

    private void CollectClock(Clock clock, int timeCount)
    {
        _timer.AddMinute(timeCount);
        clock.OnCollected -= CollectClock;
    }
}

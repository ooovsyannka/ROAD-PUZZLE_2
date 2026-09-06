using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Chains _chains;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private WinGameScreen _winGameScreen;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _earnedCoinText;
    [SerializeField] private Button _hintButton;
    [SerializeField] private Hint _hint;
    [SerializeField] private LevelSaver _levelSaver;
    [SerializeField] private float _carPositionY = 5;

    private List<StartRoad> _startRoads;
    private List<FinishRoad> _finishRoads;
    private List<RoadNode> _roads;
    private Grid _grid;
    private LevelData _currentLevelData;
    private int _completeRoadCount;

    private void OnEnable()
    {
        _timer.TimeIsOvered += Faild;
        _hintButton.onClick.AddListener(AttemptShowHint);
    }

    private void OnDisable()
    {
        _timer.TimeIsOvered -= Faild;
        _hintButton.onClick.RemoveListener(AttemptShowHint);
    }

    public void SetLevelInfo(Grid grid, List<StartRoad> startRoads, List<FinishRoad> finishRoads, List<RoadNode> roads,
        LevelData levelData)
    {
        _grid = grid;
        _roads = roads;
        _startRoads = startRoads;
        _finishRoads = finishRoads;

        foreach (FinishRoad finishRoad in _finishRoads)
        {
            if (finishRoad is IFinishRoad finish)
            {
                finish.OnConnected += TryCompleteLevel;
            }
        }

        _endGameScreen.SetLevelMode(LevelMode.Classic, levelData);
        _timer.SetMaxMinute(levelData.TimeForLevelInMinute);
        _timer.SetMaxSecond(levelData.TimeForLevelInSeconds);

        _currentLevelData = levelData;
        _hint.SetCost(levelData.HintSelection.Cost);
        _hint.SetSprite(levelData.HintSelection.HintImage);
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        Cell cell;
        Vector3 carSpawnPosition;
        Vector3 cellPosition;

        foreach (StartRoad startRoad in _startRoads)
        {
            cellPosition = startRoad.transform.position;

            if (_grid.TryGetCell(cellPosition, out cell))
            {
                cell.SetRoadNode(startRoad);
                carSpawnPosition = startRoad.transform.position;
                Car car = _carSpawner.GetRandomCar(carSpawnPosition + Vector3.up);
                car.transform.rotation = startRoad.transform.rotation;
                car.UpdateHeadlightsBasedOnTime(_currentLevelData.TimeOfDay);
                _chains.CreateChain(startRoad);
                startRoad.SetCar(car);
            }
        }

        foreach (FinishRoad finishRoad in _finishRoads)
        {
            cellPosition = finishRoad.transform.position;

            if (_grid.TryGetCell(cellPosition, out cell))
            {
                cell.SetRoadNode(finishRoad);
            }
        }

        foreach (RoadNode road in _roads)
        {
            foreach (SingleRoad singleRoad in road.SingleRoadHolder.SingleRoads)
            {
                cellPosition = singleRoad.transform.position;
                
                if (_grid.TryGetCell(cellPosition, out cell))
                {
                    cell.SetRoad(road);
                }
            }
        }

        _timer.LaunchCountdown();
    }

    private void TryCompleteLevel(IFinishRoad iFinishRoad)
    {
        Car car = null;
        bool allRoadIsConnet = true;

        if (iFinishRoad is FinishRoad finishRoad)
        {
            foreach (RoadNode road in _roads)
            {
                if (road.IsConnect == false)
                {
                    allRoadIsConnet = false;

                    break;
                }
            }

            foreach (StartRoad startRoad in _startRoads)
            {
                if (finishRoad.Index == startRoad.Index)
                {
                    if (startRoad is IStartRoad iStartRoad)
                    {
                        car = startRoad.Car;
                        car.Move(_chains.CreateRoute(iStartRoad, finishRoad.SplineComputer).SplineComputer,
                            finishRoad.SplineComputer);
                        _completeRoadCount++;

                        break;
                    }
                }
            }
        }

        if (_completeRoadCount == _startRoads.Count)
        {
            if (allRoadIsConnet)
            {
                _inputReader.StopReadInput();
                _timer.StopCountdown();
                _levelSaver.SaveLevel(_currentLevelData);
                car.MoveFinished += Win;
            }
            else
            {
                Faild("НЕ ВСЕ ДОРОГИ СОЕДЕНЕННЫ!");
            }
        }
    }

    private void Faild(string textFinishGame)
    {
        _endGameScreen.Open();
        _endGameScreen.ShowLoosInfo(textFinishGame);
        _inputReader.StopReadInput();
    }

    private void Win(Car car)
    {
        _winGameScreen.Open();
        _currentLevelData.CompleteLevel();
        car.MoveFinished -= Win;
        _wallet.AddCoin(_currentLevelData.WinCoinCount);
        _earnedCoinText.text = _currentLevelData.WinCoinCount.ToString();
    }

    private void AttemptShowHint()
    {
        if (_hint.IsBought == false)
        {
            if (_wallet.TryRemoveCoin(_hint.Cost))
            {
                _hint.Open();
            }
            else
            {
                print("Предлодить просмотр рекламы за подсказку ");
            }
        }
        else
        {
            _hint.Open();
        }
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
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
    [SerializeField] private LevelInfoBar _levelInfoBar;
    [SerializeField] private float _carPositionY = 5;
    [SerializeField] private CarProductSaver _carProductSaver;
    [SerializeField] private ProcentageUnblockingCarRender _procentageUnblockingCarRender;
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

        _levelInfoBar.UpdateLevelNumber(levelData.Index);
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

            if (!_grid.TryGetCell(cellPosition, out cell))
                continue;

            cell.SetRoadBoundary(startRoad);
            carSpawnPosition = startRoad.transform.position;
            _carSpawner.InstalSelectedCar();
            Car car = _carSpawner.GetRandomCar(carSpawnPosition + Vector3.up, startRoad.transform.rotation,
                _currentLevelData.TimeOfDay);
            _chains.CreateChain(startRoad);
            startRoad.SetCar(car);
        }

        foreach (FinishRoad finishRoad in _finishRoads)
        {
            cellPosition = finishRoad.transform.position;

            if (_grid.TryGetCell(cellPosition, out cell))
            {
                cell.SetRoadBoundary(finishRoad);
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
                if (road.IsConnect)
                    continue;

                allRoadIsConnet = false;

                break;
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
                Win();
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
        _hintButton.gameObject.SetActive(false);
    }

     private void Win()
        {
                
            _levelSaver.SaveLevel(_currentLevelData);
            int winCoinCount = _currentLevelData.WinCoinCount;
            float procentageUnblockingCar = _currentLevelData.ProcentageUnblockingCar;
            print($"{procentageUnblockingCar} procentageUnblockingCar");
            _winGameScreen.Open();
           // _currentLevelData.CompleteLevel();
            _wallet.AddCoin(winCoinCount);
            _earnedCoinText.text = winCoinCount.ToString();
            _hintButton.gameObject.SetActive(false);
            _carProductSaver.SaveProcentageUnblockingCar(procentageUnblockingCar);
            
            _procentageUnblockingCarRender.UpdateSlider(_carProductSaver.GetProcentageUnblockingCar());
        }

    private void AttemptShowHint()
    {
        if (_hint.IsBought == false)
        {
            if (_wallet.TryRemoveCoin(_hint.Cost))
            {
                _hint.Open();
                _hint.Buy();
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
using System.Collections.Generic;
using UnityEngine;

public class InfinityLevel : MonoBehaviour
{
    [SerializeField] private List<UniversalRoadBoundary> _universalRoadNodes;
    [SerializeField] private Chains _chains;
    [SerializeField] private RoadBoundaryRandomizer _roadBoundaryRandomizer;
    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private LightCycle _lightCycle;
    [SerializeField] private RoadBuilder _roadBuilder;
    [SerializeField] private Timer _timer;
    [SerializeField] private Grid _grid;
    [SerializeField] private BonusCollectionHandler _bonusCollectionHandler;
    [SerializeField] private GameOverHandler  _gameOverHandler;

   [SerializeField] private int _startLevelTime = 1;
    private bool _carIsMoving;
    private IStartRoad _currentStartRodNode;
    private UniversalRoadBoundary _currentUniversalStartRoad;

    private void Awake()
    {
        _carSpawner.InstalSelectedCar();
        _gameOverHandler.Initialize(_chains, _grid, _timer, _bonusCollectionHandler );
        _bonusCollectionHandler.Initialize(_grid, _timer);
        _roadBoundaryRandomizer.Initialize(_universalRoadNodes);
    }

    private void OnEnable()
    {
        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            if (universalRoadNode is IFinishRoad universalFinishRoad)
            {
                universalFinishRoad.OnConnected += TryCompleteChain;
            }
        }

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
    }

    private void Start()
    {
        _grid.InitializeGrid();

        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        {
            if (_grid.TryGetCell(universalRoadNode.transform.position, out Cell cell) == false)
                continue;

            cell.SetRoadBoundary(universalRoadNode);
            _chains.CreateChain(universalRoadNode);
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

    /*public void SetGrid(Grid grid)
    {
        _grid = grid;
    }*/

    private void NextMove()
    {
        _currentUniversalStartRoad = _roadBoundaryRandomizer.GetStartRoad();
        Car car = _carSpawner.GetRandomCar(_currentUniversalStartRoad.transform.position + Vector3.up,
            _currentUniversalStartRoad.transform.rotation, _lightCycle.TimeOfDay);
        _currentUniversalStartRoad.SetCar(car);
        _currentStartRodNode = _currentUniversalStartRoad;
        _roadBoundaryRandomizer.TryBreakUniversalFinishRoad();
        _bonusCollectionHandler.AttemptSpawnBonus();
    }

    private void TryCompleteChain(IFinishRoad finishRoad)
    {
        if (_carIsMoving || finishRoad is not UniversalRoadBoundary universalFinishRoad)
            return;

        Car car = _currentUniversalStartRoad.Car;
        if (car == null)
            return;

        _carIsMoving = true;

        Route route = _chains.CreateRoute(
            _currentStartRodNode,
            universalFinishRoad.SplineComputer);

        car.Move(route.SplineComputer, universalFinishRoad.SplineComputer);
        car.MoveFinished += CleanChain;
        car.MoveFinished += route.CleanSplineComputer;
    }

    private void CleanChain(Car car)
    {
        car.MoveFinished -= CleanChain;
        _chains.CleanChain(_currentUniversalStartRoad, _grid);
        _lightCycle.TryChangeLightIntensity();

        NextMove();
        car.Die();

        _carIsMoving = false;
    }
}
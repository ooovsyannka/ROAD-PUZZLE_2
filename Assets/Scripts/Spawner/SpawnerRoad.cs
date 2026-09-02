using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoad : MonoBehaviour
{
    private const float CurveRoadChance = 0.6f;

    [SerializeField] private Grid _grid;
    [SerializeField] private List<Quaternion> _quaternions;
    [SerializeField] private Road _roadPrefab;
    [SerializeField] private CurveSample _curvePrevab;
    [SerializeField] private StraightSample _straightPrefab;
    [SerializeField] private MergePoint _mergePointPrefab;
    [SerializeField] private List<Cell> _roadSpawnPoints;
    [SerializeField] private RoadDrager _roadDrager;

    private Spawner<CurveSample> _spawnerCurve;
    private Spawner<StraightSample> _spawnerStraight;
    private Spawner<Road> _spawnerRoad;

    private int _pastQuaternionIndex = 0;

    private int indexRoad = 0;

    private List<Road> _tempRoads = new List<Road>();

    private void Awake()
    {
        _spawnerRoad = new Spawner<Road>(_roadPrefab);
        _spawnerCurve = new Spawner<CurveSample>(_curvePrevab);
        _spawnerStraight = new Spawner<StraightSample>(_straightPrefab);
    }

    private void OnEnable()
    {
        _roadDrager.DragOver += CreateRoad;
    }

    private void Start()
    {
        CreateRoad();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grid.TryGetCell(_grid.TryGetRandomEmptyCell().transform.position, out Cell cell))
            {
                Road road = _spawnerRoad.Spawn(cell.transform.position, null);
                _tempRoads.Add(road);
                cell.SetRoad(road);

                string name;

                float sampleRoadChance = Random.Range(0, 1f);
                SampleSingleRoad sampleSingleRoad;

                if (sampleRoadChance < CurveRoadChance)
                {
                    sampleSingleRoad = _spawnerStraight.Spawn(road.transform.position, road.transform);
                    name = "Straight";
                }
                else
                {
                    sampleSingleRoad = _spawnerCurve.Spawn(road.transform.position, road.transform);
                    name = "Curve";
                }
                indexRoad++;
                InitializeRoad(road, sampleSingleRoad);
                SinglePreview singlePreview = Instantiate(sampleSingleRoad.SinglePreview);
                road.SetRoadPreview(singlePreview);
                singlePreview.transform.localPosition = Vector3.zero;
                road.transform.position = cell.transform.position;
                road.name = road.name + name + indexRoad.ToString();
            }
        }
    }

    private void CreateRoad(Road _ = null)
    {
        bool canSpawRoad = true;

        foreach (Cell cell in _roadSpawnPoints)
        {
            if (cell.IsFree == false)
            {
                canSpawRoad = false;

                break;
            }
        }

        if (canSpawRoad)
        {
            foreach (Cell cell in _roadSpawnPoints)
            {
                Road road = _spawnerRoad.Spawn(cell.transform.position, null);
                _tempRoads.Add(road);
                cell.SetRoad(road);
                string name;
                float sampleRoadChance = Random.Range(0, 1f);
                SampleSingleRoad sampleSingleRoad;

                if (sampleRoadChance < CurveRoadChance)
                {
                    sampleSingleRoad = _spawnerStraight.Spawn(road.transform.position, road.transform);
                    name = "Straight";
                }
                else
                {
                    sampleSingleRoad = _spawnerCurve.Spawn(road.transform.position, road.transform);
                    name = "Curve";
                }
                indexRoad++;
                InitializeRoad(road, sampleSingleRoad);
                SinglePreview singlePreview = Instantiate(sampleSingleRoad.SinglePreview);
                road.SetRoadPreview(singlePreview);
                singlePreview.transform.localPosition = Vector3.zero;
                road.transform.position = cell.transform.position;
                road.name = road.name + name + indexRoad.ToString();
            }
        }
    }

    private void InitializeRoad(Road road, SampleSingleRoad sampleSingleRoad)
    {
        TurnRoad(sampleSingleRoad);
        road.AddRoad(sampleSingleRoad.SingleRoadPrefab);
        road.AddMergePoint(sampleSingleRoad.ExitPoint, sampleSingleRoad.EnterPoint);
    }

    private void TurnRoad(SampleSingleRoad currentRoad)
    {
        if (currentRoad == null)
            return;

        int indexQuaternion = Random.Range(0, _quaternions.Count);
        Quaternion currentQuaternion = _quaternions[indexQuaternion];

        if (indexQuaternion == _pastQuaternionIndex)
        {
            if (indexQuaternion != _quaternions.Count - 1)
            {
                indexQuaternion++;
            }
            else
            {
                indexQuaternion = 0;
            }

            currentQuaternion = _quaternions[indexQuaternion];
        }

        _pastQuaternionIndex = indexQuaternion;
        currentRoad.transform.rotation = currentQuaternion;
    }

}
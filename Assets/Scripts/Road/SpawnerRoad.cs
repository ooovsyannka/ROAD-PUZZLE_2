using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerRoad : MonoBehaviour
{
    private const float CurveRoadChance = 0.5f;

    [SerializeField] private Grid _grid;
    [SerializeField] private List<Quaternion> _quaternions;

    [SerializeField] private RoadNode _roadNodePrefab;

    [SerializeField] private CurveSample _curvePrevab;
    [SerializeField] private StraightSample _straightPrefab;
    [SerializeField] private MergePoint _mergePointPrefab;
    [SerializeField] private List<Cell> _roadSpawnPoints;
    [SerializeField] private RoadDrager _roadDrager;

    private Spawner<CurveSample> _spawnerCurve;
    private Spawner<StraightSample> _spawnerStraight;
    private Spawner<RoadNode> _spawnerRoad;

    private int _pastQuaternionIndex = 0;

    private int indexRoad = 0;

    private List<RoadNode> _tempRoads = new List<RoadNode>();

    private void Awake()
    {
        _spawnerRoad = new Spawner<RoadNode>(_roadNodePrefab);
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
        if (Input.GetKeyDown(KeyCode.Space) == false ||
            _grid.TryGetCell(_grid.TryGetRandomEmptyCell().transform.position, out Cell cell) == false)
            return;

        RoadNode roadNode = _spawnerRoad.Spawn(cell.transform.position, null);
        _tempRoads.Add(roadNode);
        cell.SetRoad(roadNode);

        string name;

        float sampleRoadChance = Random.Range(0, 1f);
        SampleSingleRoad sampleSingleRoad;

        if (sampleRoadChance < CurveRoadChance)
        {
            sampleSingleRoad = _spawnerStraight.Spawn(roadNode.transform.position, roadNode.transform);
            name = "Straight";
        }
        else
        {
            sampleSingleRoad = _spawnerCurve.Spawn(roadNode.transform.position, roadNode.transform);
            name = "Curve";
        }

        indexRoad++;
        InitializeRoad(roadNode, sampleSingleRoad);
        SinglePreview singlePreview = Instantiate(sampleSingleRoad.SinglePreview);
        roadNode.SetRoadPreview(singlePreview);
        singlePreview.transform.localPosition = Vector3.zero;
        roadNode.transform.position = cell.transform.position;
        roadNode.name = roadNode.name + name + indexRoad.ToString();
    }

    /*private void CreateRoad(RoadNode _ = null)
    {
        bool canSpawRoad = true;

        foreach (Cell cell in _roadSpawnPoints)
        {
            if (cell.IsFree)
                continue;

            canSpawRoad = false;

            break;
        }

        if (canSpawRoad)
        {
            foreach (Cell cell in _roadSpawnPoints)
            {
                RoadNode roadNode = _spawnerRoad.Spawn(cell.transform.position, null);
                _tempRoads.Add(roadNode);
                cell.SetRoad(roadNode);
                string name;
                float sampleRoadChance = Random.Range(0, 1f);
                SampleSingleRoad sampleSingleRoad;

                if (sampleRoadChance < CurveRoadChance)
                {
                    sampleSingleRoad = _spawnerStraight.Spawn(roadNode.transform.position, roadNode.transform);
                    name = "Straight";
                }
                else
                {
                    sampleSingleRoad = _spawnerCurve.Spawn(roadNode.transform.position, roadNode.transform);
                    name = "Curve";
                }

                indexRoad++;
                InitializeRoad(roadNode, sampleSingleRoad);
                SinglePreview singlePreview = Instantiate(sampleSingleRoad.SinglePreview);
                roadNode.SetRoadPreview(singlePreview);
                singlePreview.transform.localPosition = Vector3.zero;
                roadNode.transform.position = cell.transform.position;
                roadNode.name = roadNode.name + name + indexRoad.ToString();
            }
        }
    }*/

    private void CreateRoad(RoadNode _ = null)
    {
        foreach (Cell cell in _roadSpawnPoints)
        {
            if (cell.IsFree)
            {
                RoadNode roadNode = _spawnerRoad.Spawn(cell.transform.position, null);
                _tempRoads.Add(roadNode);
                cell.SetRoad(roadNode);
                string name;
                float sampleRoadChance = Random.Range(0, 1f);
                SampleSingleRoad sampleSingleRoad;

                if (sampleRoadChance < CurveRoadChance)
                {
                    sampleSingleRoad = _spawnerStraight.Spawn(roadNode.transform.position, roadNode.transform);
                    name = "Straight";
                }
                else
                {
                    sampleSingleRoad = _spawnerCurve.Spawn(roadNode.transform.position, roadNode.transform);
                    name = "Curve";
                }

                indexRoad++;
                InitializeRoad(roadNode, sampleSingleRoad);
                SinglePreview singlePreview = Instantiate(sampleSingleRoad.SinglePreview);
                roadNode.SetRoadPreview(singlePreview);
                singlePreview.transform.localPosition = Vector3.zero;
                roadNode.transform.position = cell.transform.position;
                roadNode.name = roadNode.name + name + indexRoad.ToString();
            }
        }
    }

    private void InitializeRoad(RoadNode roadNode, SampleSingleRoad sampleSingleRoad)
    {
        TurnRoad(sampleSingleRoad);
        roadNode.AddRoad(sampleSingleRoad.SingleRoadPrefab);
        roadNode.AddMergePoint(sampleSingleRoad.ExitPoint, sampleSingleRoad.EnterPoint);
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
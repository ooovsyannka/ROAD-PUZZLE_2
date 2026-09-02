using UnityEngine;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

public class RoadInstantiator : MonoBehaviour
{
    [SerializeField] private Road _roadPrefab;
    [SerializeField] private StartRoad _startRoadPrefab;
    [SerializeField] private FinishRoad _finishRoadPrefab;
    [SerializeField] private CurveSample _curveSample;
    [SerializeField] private StraightSample _straightSample;
    [SerializeField] private MergePoint _mergePointPrefab;

    private List<Road> _roads = new List<Road>();
    private List<StartRoad> _startRoads = new List<StartRoad>();
    private List<FinishRoad> _finishRoads = new List<FinishRoad>();

    public List<Road> Roads => _roads;
    public List<StartRoad> StartRoads => _startRoads;
    public List<FinishRoad> FinishRoads => _finishRoads;

    public void InstantiateRoads(LevelData levelData)
    {
        TryInstantiateSingleRoad(levelData.SingleRoadSections);
        TryInstantiateMultiRoad(levelData.MultiRoadSections);
        TryInstantiateStartRoad(levelData.StartRoadSections);
        TryInstantiateFinishRoad(levelData.FinishRoadSections);
    }

    private void TryInstantiateStartRoad(List<StartRoadSection> startRoadSections)
    {
        if (startRoadSections != null)
        {
            foreach (StartRoadSection startRoadSection in startRoadSections)
            {
                StartRoad startRoad = Instantiate(_startRoadPrefab, startRoadSection.RoadSectionTransform.Position, startRoadSection.RoadSectionTransform.Rotation);
                startRoad.SetIndex(startRoadSection.Index);
                _startRoads.Add(startRoad);
            }
        }
    }

    private void TryInstantiateFinishRoad(List<FinishRoadSection> finishRoadSections)
    {
        if (finishRoadSections != null)
        {
            foreach (FinishRoadSection finishRoadSection in finishRoadSections)
            {
                FinishRoad finishRoad = Instantiate(_finishRoadPrefab, finishRoadSection.RoadSectionTransform.Position, finishRoadSection.RoadSectionTransform.Rotation);
                finishRoad.SetIndex(finishRoadSection.Index);
                _finishRoads.Add(finishRoad);
            }
        }
    }

    private void TryInstantiateMultiRoad(List<MultiRoadSection> multiRoadSections)
    {
        SingleRoad singleRoad = null;

        if (multiRoadSections != null)
        {
            if (multiRoadSections.Count > 0)
            {
                foreach (MultiRoadSection multiRoadSection in multiRoadSections)
                {
                    Road road = Instantiate(_roadPrefab);

                    foreach (SingleRoadSection roadSection in multiRoadSection.SingleRoadSections)
                    {
                        if (roadSection.RoadType == RoadType.Curve)
                        {
                            singleRoad = InstantiateCurveRoad(road);
                        }
                        else if (roadSection.RoadType == RoadType.Straight)
                        {
                            singleRoad = InstantiateStraightRoad(road);
                        }

                        if (singleRoad != null)
                        {
                            singleRoad.transform.localPosition = roadSection.RoadSectionTransforms.Position;
                            singleRoad.transform.localRotation = roadSection.RoadSectionTransforms.Rotation;
                            road.SetRoadPreview(Instantiate(_curveSample.SinglePreview, singleRoad.transform.localPosition, Quaternion.identity));
                            road.AddRoad(singleRoad);
                        }
                    }

                    road.AddMergePoint(InstantiateMergePoint(multiRoadSection.EnterPointPosition, road),
                            InstantiateMergePoint(multiRoadSection.ExitPointPosition, road));
                    road.transform.position = multiRoadSection.RoadSectionTransforms.Position;
                    road.transform.rotation = multiRoadSection.RoadSectionTransforms.Rotation;
                    road.RoadRotation.SetInitialRotation();
                    _roads.Add(road);
                }
            }
        }
    }

    private void TryInstantiateSingleRoad(List<SingleRoadSection> singleRoadSections)
    {
        if (singleRoadSections != null)
        {
            if (singleRoadSections.Count > 0)
            {
                foreach (SingleRoadSection roadSection in singleRoadSections)
                {
                    Road road = Instantiate(_roadPrefab);

                    if (roadSection.RoadType == RoadType.Curve)
                    {
                        road.AddRoad(InstantiateCurveRoad(road));
                        road.AddMergePoint(InstantiateMergePoint(_curveSample.EnterPoint.transform.localPosition, road),
                            InstantiateMergePoint(_curveSample.ExitPoint.transform.localPosition, road));
                        road.SetRoadPreview(Instantiate(_curveSample.SinglePreview));
                    }
                    else if (roadSection.RoadType == RoadType.Straight)
                    {
                        road.AddRoad(InstantiateStraightRoad(road));
                        road.AddMergePoint(InstantiateMergePoint(_straightSample.EnterPoint.transform.localPosition, road),
                            InstantiateMergePoint(_straightSample.ExitPoint.transform.localPosition, road));
                        road.SetRoadPreview(Instantiate(_straightSample.SinglePreview));
                    }

                    road.transform.position = roadSection.RoadSectionTransforms.Position;
                    road.transform.rotation = roadSection.RoadSectionTransforms.Rotation;
                    road.RoadRotation.SetInitialRotation();
                    _roads.Add(road);
                }
            }
        }
    }

    private SingleRoad InstantiateCurveRoad(Road road) =>
        Instantiate(_curveSample.SingleRoadPrefab, road.SingleRoadHolder.transform);

    private SingleRoad InstantiateStraightRoad(Road road) =>
        Instantiate(_straightSample.SingleRoadPrefab, road.SingleRoadHolder.transform);

    private MergePoint InstantiateMergePoint(Vector3 localPosition, Road road) =>
        Instantiate(_mergePointPrefab, localPosition,
                            Quaternion.identity, road.MergePointHolder.transform);
}

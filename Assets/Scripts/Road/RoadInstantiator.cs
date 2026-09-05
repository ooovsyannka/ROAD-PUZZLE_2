using UnityEngine;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEngine.Serialization;

public class RoadInstantiator : MonoBehaviour
{
    [FormerlySerializedAs("roadNodeTempPrefab")] [FormerlySerializedAs("_roadPrefab")] [SerializeField] private RoadNode roadNodePrefab;
    [SerializeField] private StartRoad _startRoadPrefab;
    [SerializeField] private FinishRoad _finishRoadPrefab;
    [SerializeField] private CurveSample _curveSample;
    [SerializeField] private StraightSample _straightSample;
    [SerializeField] private MergePoint _mergePointPrefab;

    private List<RoadNode> _roads = new List<RoadNode>();
    private List<StartRoad> _startRoads = new List<StartRoad>();
    private List<FinishRoad> _finishRoads = new List<FinishRoad>();

    public List<RoadNode> Roads => _roads;
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
                    RoadNode roadNode = Instantiate(roadNodePrefab);

                    foreach (SingleRoadSection roadSection in multiRoadSection.SingleRoadSections)
                    {
                        if (roadSection.RoadNodeType == RoadNodeType.Curve)
                        {
                            singleRoad = InstantiateCurveRoad(roadNode);
                        }
                        else if (roadSection.RoadNodeType == RoadNodeType.Straight)
                        {
                            singleRoad = InstantiateStraightRoad(roadNode);
                        }

                        if (singleRoad != null)
                        {
                            singleRoad.transform.localPosition = roadSection.RoadSectionTransforms.Position;
                            singleRoad.transform.localRotation = roadSection.RoadSectionTransforms.Rotation;
                            roadNode.SetRoadPreview(Instantiate(_curveSample.SinglePreview, singleRoad.transform.localPosition, Quaternion.identity));
                            roadNode.AddRoad(singleRoad);
                        }
                    }

                    roadNode.AddMergePoint(InstantiateMergePoint(multiRoadSection.EnterPointPosition, roadNode),
                            InstantiateMergePoint(multiRoadSection.ExitPointPosition, roadNode));
                    roadNode.transform.position = multiRoadSection.RoadSectionTransforms.Position;
                    roadNode.transform.rotation = multiRoadSection.RoadSectionTransforms.Rotation;
                    roadNode.RoadRotation.SetInitialRotation();
                    _roads.Add(roadNode);
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
                    RoadNode roadNode = Instantiate(roadNodePrefab);

                    if (roadSection.RoadNodeType == RoadNodeType.Curve)
                    {
                        roadNode.AddRoad(InstantiateCurveRoad(roadNode));
                        roadNode.AddMergePoint(InstantiateMergePoint(_curveSample.EnterPoint.transform.localPosition, roadNode),
                            InstantiateMergePoint(_curveSample.ExitPoint.transform.localPosition, roadNode));
                        roadNode.SetRoadPreview(Instantiate(_curveSample.SinglePreview));
                    }
                    else if (roadSection.RoadNodeType == RoadNodeType.Straight)
                    {
                        roadNode.AddRoad(InstantiateStraightRoad(roadNode));
                        roadNode.AddMergePoint(InstantiateMergePoint(_straightSample.EnterPoint.transform.localPosition, roadNode),
                            InstantiateMergePoint(_straightSample.ExitPoint.transform.localPosition, roadNode));
                        roadNode.SetRoadPreview(Instantiate(_straightSample.SinglePreview));
                    }

                    roadNode.transform.position = roadSection.RoadSectionTransforms.Position;
                    roadNode.transform.rotation = roadSection.RoadSectionTransforms.Rotation;
                    roadNode.RoadRotation.SetInitialRotation();
                    _roads.Add(roadNode);
                }
            }
        }
    }

    private SingleRoad InstantiateCurveRoad(RoadNode roadNode) =>
        Instantiate(_curveSample.SingleRoadPrefab, roadNode.SingleRoadHolder.transform);

    private SingleRoad InstantiateStraightRoad(RoadNode roadNode) =>
        Instantiate(_straightSample.SingleRoadPrefab, roadNode.SingleRoadHolder.transform);

    private MergePoint InstantiateMergePoint(Vector3 localPosition, RoadNode roadNode) =>
        Instantiate(_mergePointPrefab, localPosition,
                            Quaternion.identity, roadNode.MergePointHolder.transform);
}

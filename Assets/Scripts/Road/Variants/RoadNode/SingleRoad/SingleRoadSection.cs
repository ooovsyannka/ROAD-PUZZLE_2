using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SingleRoadSection
{
    [FormerlySerializedAs("_roadType")] [SerializeField] private RoadNodeType roadNodeType;
    [SerializeField] private TransformSection _roadSectionTransforms;

    public RoadNodeType RoadNodeType => roadNodeType;
    public TransformSection RoadSectionTransforms => _roadSectionTransforms;
}

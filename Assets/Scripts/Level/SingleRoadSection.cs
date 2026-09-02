using UnityEngine;

[System.Serializable]
public class SingleRoadSection
{
    [SerializeField] private RoadType _roadType;
    [SerializeField] private TransformSection _roadSectionTransforms;

    public RoadType RoadType => _roadType;
    public TransformSection RoadSectionTransforms => _roadSectionTransforms;
}

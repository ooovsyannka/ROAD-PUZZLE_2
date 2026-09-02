using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiRoadSection
{
    [SerializeField] private TransformSection _roadSectionTransforms;
    [SerializeField] private List<SingleRoadSection> _roadType;
    [SerializeField] private Vector3 _enterPointPosition;
    [SerializeField] private Vector3 _exitPointPosition;

    public List<SingleRoadSection> SingleRoadSections => _roadType;
    public TransformSection RoadSectionTransforms => _roadSectionTransforms;
    public Vector3 EnterPointPosition => _enterPointPosition;
    public Vector3 ExitPointPosition => _exitPointPosition;
}

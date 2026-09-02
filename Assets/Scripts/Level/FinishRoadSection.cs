using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class FinishRoadSection
{
    [SerializeField] private int _index;
    [SerializeField] private TransformSection _roadSectionTransform;

    public int Index => _index;
    public TransformSection RoadSectionTransform => _roadSectionTransform;
}
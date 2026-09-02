using UnityEngine;

[System.Serializable]
public class StartRoadSection
{
    [SerializeField] private int _index;
    [SerializeField] private TransformSection _roadSectionTransform;

    public  int Index => _index;
    public TransformSection RoadSectionTransform => _roadSectionTransform;
}

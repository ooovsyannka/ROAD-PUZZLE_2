using Dreamteck.Splines;
using UnityEngine;

public abstract class SampleSingleRoad : MonoBehaviour
{
    [SerializeField] private SingleRoad _singleRoadPrefab;
    [SerializeField] private SinglePreview _singlePreview;
    [SerializeField] private MergePoint _enterPoint;
    [SerializeField] private MergePoint _exitPoint;
    
    public SinglePreview SinglePreview => _singlePreview;
    public SingleRoad SingleRoadPrefab => _singleRoadPrefab;
    public MergePoint EnterPoint => _enterPoint;
    public MergePoint ExitPoint => _exitPoint;
}

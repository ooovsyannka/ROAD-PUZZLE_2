using System.Collections.Generic;
using UnityEngine;

public class RoadBoundaryRandomizer : MonoBehaviour
{
    [SerializeField] private List<UniversalRoadBoundary> _universalRoadNodes;
    [SerializeField, Range(0f, 1f)] private float _breakProbability = 0.4f;

    private UniversalRoadBoundary _brokenUniversalRoad;

    public void TryBreakUniversalFinishRoad()
    {
        if (_universalRoadNodes == null || _universalRoadNodes.Count == 0)
            return;

        if (_brokenUniversalRoad != null)
        {
            _brokenUniversalRoad.Fix();
            _brokenUniversalRoad = null;
        }

        if (Random.value < _breakProbability)
            return;

        List<UniversalRoadBoundary> finishRoads = new();

        foreach (UniversalRoadBoundary road in _universalRoadNodes)
        {
            if (road != null && road.RoadBoundaryType == RoadBoundaryType.Finish)
                finishRoads.Add(road);
        }

        if (finishRoads.Count == 0)
            return;

        _brokenUniversalRoad = finishRoads[Random.Range(0, finishRoads.Count)];
        _brokenUniversalRoad.Broken();
    }

    public UniversalRoadBoundary GetStartRoad()
    {
        if (_universalRoadNodes == null || _universalRoadNodes.Count == 0)
            return null;

        SetAllRoadsAsFinish();

        UniversalRoadBoundary startRoad =
            _universalRoadNodes[Random.Range(0, _universalRoadNodes.Count)];

        if (startRoad == null)
            return null;

        startRoad.SetStarType();
        return startRoad;
    }

    private void SetAllRoadsAsFinish()
    {
        foreach (UniversalRoadBoundary road in _universalRoadNodes)
        {
            if (road != null)
                road.SetFinishType();
        }
    }
}
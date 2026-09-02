using Dreamteck.Splines;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private SplineComputer _splineComputer;

    public SplineComputer SplineComputer => _splineComputer;

    public event Action<Route> Cleaned;

    public void CleanSplineComputer(Car car)
    {
        SplinePoint[] splinePoints = new SplinePoint[0];

        _splineComputer.SetPoints(splinePoints);
        _splineComputer.Rebuild();
        car.MoveFinished -= CleanSplineComputer;

        Cleaned?.Invoke(this);
    }

    public void JoinSpline(IStartRoad iStartRoad, SplineComputer finishRoadSpline, List<Road> roads)
    {
        List<SingleRoad> singleRoads;
        Vector3 lastPosition;

        if (iStartRoad is RoadNode startRoadNode)
        {
            lastPosition = startRoadNode.transform.position;

            AddSpline(startRoadNode.SplineComputer);

            foreach (Road road in roads)
            {
                singleRoads = road.SingleRoadHolder.SingleRoads;
                road.StopDrag();

                if (singleRoads.Count > 1)
                {
                    singleRoads = singleRoads.OrderBy(singleRoad =>
                                              Vector3.Distance(lastPosition,
                                              singleRoad.transform.position)).ToList();

                    foreach (SingleRoad singleRoad in singleRoads)
                    {
                        AddSpline(singleRoad.SplineComputer);
                    }

                    lastPosition = singleRoads[singleRoads.Count - 1].transform.position;
                }
                else
                {
                    AddSpline(road.SingleRoadHolder.SingleRoads[0].SplineComputer);
                    lastPosition = road.SingleRoadHolder.SingleRoads[0].transform.position;
                }
            }

            AddSpline(finishRoadSpline);

            _splineComputer.Rebuild();
        }
    }

    private void AddSpline(SplineComputer splineComputer)
    {
        SplinePoint[] newPoints = splineComputer.GetPoints();
        int startIndex = _splineComputer.pointCount;

        for (int i = 0; i < newPoints.Length; i++)
        {
            SplinePoint splinePoint = newPoints[i];
            _splineComputer.SetPoint(startIndex + i, splinePoint);
        }
    }
}
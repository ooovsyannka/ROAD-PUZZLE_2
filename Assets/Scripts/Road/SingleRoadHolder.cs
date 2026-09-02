using System.Collections.Generic;
using UnityEngine;

public class SingleRoadHolder : MonoBehaviour
{
    [SerializeField] private List<SingleRoad> _singleRoads;

    public List<SingleRoad> SingleRoads => _singleRoads;

    public void AddSingleRoad(SingleRoad singleRoad)
    {
        _singleRoads.Add(singleRoad);
    }

    public void DisconnectSingleRoad()
    {
        foreach (SingleRoad road in _singleRoads)
        {
            road.Disconnect();
        }
    }
    public void ConnectSingleRoad()
    {
        foreach (SingleRoad road in _singleRoads)
        {
            road.Connect();
        }
    }
}
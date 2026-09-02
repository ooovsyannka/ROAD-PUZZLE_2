using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chains : MonoBehaviour
{
    [SerializeField] private RouteSpawner _routeSpawner;

    private Dictionary<IStartRoad, List<Road>> _chains;
    private float _timeDelay = 0.075f;
    private Coroutine _removeChain;
    private WaitForSeconds _delay;
    private Queue<Route> _routes = new Queue<Route>();
    private bool _chainCopmete;

    public bool ChainCopmete => _chainCopmete;

    public event Action<List<Road>> ChainCompleted;
    public event Action<IStartRoad> RoadsOver;

    private void Awake()
    {
        if (_chains == null)
            _chains = new Dictionary<IStartRoad, List<Road>>();

        _delay = new WaitForSeconds(_timeDelay);
    }

    public void CreateChain(IStartRoad iStartRoad)
    {
        if (_chains == null)
            _chains = new Dictionary<IStartRoad, List<Road>>();

        Route route = _routeSpawner.InstantieteRoute();

        _chains.Add(iStartRoad, new List<Road>());
        _routes.Enqueue(route);
    }

    public bool TryAddRoadInChain(IStartRoad startRoad, Road attachedRoad)
    {
        if (_chains[startRoad].Count == 0)
        {
            _chains[startRoad].Add(attachedRoad);
            attachedRoad.RoadJoined += AddRoad;
            attachedRoad.Disconected += RemoveRoad;

            return true;
        }

        return false;
    }

    public Route CreateRoute(IStartRoad iStartRoad, SplineComputer finishRoadSpline)
    {
        _chainCopmete = true;
        Route route = null;
        List<Road> roads = _chains[iStartRoad];

        ChainCompleted?.Invoke(roads);

        if (_routes.Count > 0)
        {
            route = _routes.Dequeue();
            route.JoinSpline(iStartRoad, finishRoadSpline, roads);
            route.Cleaned += AddCleanRoute;
        }

        return route;
    }

    public void ClearRoad(Cell cell)
    {
        Destroy(cell.Road.gameObject);
        RemoveRoad(cell.Road);
        cell.CleanRoad();
        cell.AnimationFinished -= ClearRoad;
    }

    public void CleanChain(IStartRoad roadNode, Grid grid)
    {
        if (_removeChain != null)
            StopCoroutine(_removeChain);

        _removeChain = StartCoroutine(RemoveChain(roadNode, grid));
    }

    private void RemoveRoad(Road road)
    {
        TryRemoveRoad(road);
        road.RoadJoined -= AddRoad;
        road.Disconected -= RemoveRoad;
    }

    private void AddRoad(Road roadInChain, Road road)
    {
        List<Road> targetLists = null;

        foreach (var chain in _chains.Values)
        {
            if (chain.Contains(roadInChain))
            {
                targetLists = chain;

                break;
            }
        }

        targetLists.Add(road);

        road.RoadJoined += AddRoad;
        road.Disconected += RemoveRoad;
    }

    private void TryRemoveRoad(Road road)
    {
        IStartRoad foundKey = null;
        List<Road> targetList = null;
        Road findSingleRoad = null;

        foreach (var chain in _chains)
        {
            if (chain.Value.Contains(road))
            {
                findSingleRoad = road;
                foundKey = chain.Key;
                targetList = chain.Value;

                break;
            }
        }

        if (foundKey != null)
        {
            if (_chains[foundKey].First() == findSingleRoad)
            {
                _chains[foundKey].Clear();

                RoadsOver?.Invoke(foundKey);
            }
            else
            {
                targetList.Remove(road);
            }
        }
    }

    private IEnumerator RemoveChain(IStartRoad roadNode, Grid grid)
    {
      //  ChainCompleted?.Invoke(_chains[roadNode]);

        foreach (Road road in _chains[roadNode])
        {
            foreach (SingleRoad singleRoad in road.SingleRoadHolder.SingleRoads)
            {
                if (grid.TryGetCell(singleRoad.transform.position, out Cell cell))
                {
                    cell.PlayAnimation();
                    cell.AnimationFinished += ClearRoad;
                }
            }

            yield return _delay;
        }

        _chainCopmete = false;
    }

    private void AddCleanRoute(Route route)
    {
        route.Cleaned -= AddCleanRoute;
        _routes.Enqueue(route);
    }
}

using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chains : MonoBehaviour
{
    [SerializeField] private RouteSpawner _routeSpawner;

    private Dictionary<IStartRoad, List<RoadNode>> _chains;
    private float _timeDelay = 0.075f;
    private Coroutine _removeChain;
    private WaitForSeconds _delay;
    private Queue<Route> _routes = new Queue<Route>();
    private bool _chainCopmete;

    public bool ChainCopmete => _chainCopmete;

    public event Action<List<RoadNode>> ChainCompleted;
    public event Action<IStartRoad> RoadsOver;

    private void Awake()
    {
        if (_chains == null)
            _chains = new Dictionary<IStartRoad, List<RoadNode>>();

        _delay = new WaitForSeconds(_timeDelay);
    }

    public void CreateChain(IStartRoad iStartRoad)
    {
        if (_chains == null)
            _chains = new Dictionary<IStartRoad, List<RoadNode>>();

        Route route = _routeSpawner.InstantieteRoute();

        _chains.Add(iStartRoad, new List<RoadNode>());
        _routes.Enqueue(route);
    }

    public bool TryAddRoadInChain(IStartRoad startRoad, RoadNode attachedRoadNode)
    {
        if (_chains[startRoad].Count == 0)
        {
            _chains[startRoad].Add(attachedRoadNode);
            attachedRoadNode.RoadJoined += AddRoad;
            attachedRoadNode.Disconected += RemoveRoad;

            return true;
        }

        return false;
    }

    public Route CreateRoute(IStartRoad iStartRoad, SplineComputer finishRoadSpline)
    {
        _chainCopmete = true;
        Route route = null;
        List<RoadNode> roads = _chains[iStartRoad];

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
        Destroy(cell.RoadNode.gameObject);
        RemoveRoad(cell.RoadNode);
        cell.CleanRoad();
        cell.AnimationFinished -= ClearRoad;
    }

    public void CleanChain(IStartRoad roadNode, Grid grid)
    {
        if (_removeChain != null)
            StopCoroutine(_removeChain);

        _removeChain = StartCoroutine(RemoveChain(roadNode, grid));
    }

    private void RemoveRoad(RoadNode roadNode)
    {
        TryRemoveRoad(roadNode);
        roadNode.RoadJoined -= AddRoad;
        roadNode.Disconected -= RemoveRoad;
    }

    private void AddRoad(RoadNode roadNodeInChain, RoadNode roadNode)
    {
        List<RoadNode> targetLists = null;

        foreach (var chain in _chains.Values)
        {
            if (chain.Contains(roadNodeInChain))
            {
                targetLists = chain;

                break;
            }
        }

        targetLists.Add(roadNode);

        roadNode.RoadJoined += AddRoad;
        roadNode.Disconected += RemoveRoad;
    }

    private void TryRemoveRoad(RoadNode roadNode)
    {
        IStartRoad foundKey = null;
        List<RoadNode> targetList = null;
        RoadNode findSingleRoadNode = null;

        foreach (var chain in _chains)
        {
            if (chain.Value.Contains(roadNode))
            {
                findSingleRoadNode = roadNode;
                foundKey = chain.Key;
                targetList = chain.Value;

                break;
            }
        }

        if (foundKey != null)
        {
            if (_chains[foundKey].First() == findSingleRoadNode)
            {
                _chains[foundKey].Clear();

                RoadsOver?.Invoke(foundKey);
            }
            else
            {
                targetList.Remove(roadNode);
            }
        }
    }

    private IEnumerator RemoveChain(IStartRoad roadNode, Grid grid)
    {
      //  ChainCompleted?.Invoke(_chains[roadNode]);

        foreach (RoadNode road in _chains[roadNode])
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

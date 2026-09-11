using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private Chains _chains;
    [SerializeField] private Grid _grid;

    private List<StartRoad> _startRoads;
    private List<UniversalRoadBoundary> _universalRoadBoundarys;
    private List<FinishRoad> _finishRoads;

    private int _connectRoad = 0;

    private void OnEnable()
    {
        _roadDrager.DragOver += TryBuildRoad;
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    public void SetStartRoads(List<StartRoad> iStartRoads)
    {
        // _startRoads = iStartRoads;
        // _roadDrager.DragOver += TestStart;
    }

    public void SetFinishRoads(List<FinishRoad> finishRoads)
    {
        _finishRoads = finishRoads;
    }

    public void SetUniversalRoadNode(List<UniversalRoadBoundary> universalRoadNode)
    {
        _universalRoadBoundarys = universalRoadNode;
        _roadDrager.DragOver += TryFindNearRoad;
    }

    private void TryBuildRoad(RoadNode roadNode)
    {
        Vector3 enterPoint = roadNode.MergePointHolder.EnterPoint.transform.position;
        Vector3 exitPoint = roadNode.MergePointHolder.ExitPoint.transform.position;
        Cell cell;

        if (_grid.TryGetCell(enterPoint, out cell))
        {
            ProcessPointCheck(cell, roadNode);
        }

        if (_grid.TryGetCell(exitPoint, out cell))
        {
            ProcessPointCheck(cell, roadNode);
        }
    }

    private void TryFindNearRoad(RoadNode _)
    {
        foreach (UniversalRoadBoundary universalRoadBoundary in _universalRoadBoundarys)
        foreach (MergePoint mergePoint in universalRoadBoundary.MergePoints)
        {
            if (!_grid.TryGetCell(mergePoint.transform.position, out Cell cell)
                || !cell.TryGetRoad(out RoadNode road)
                || (road.MergePointHolder.EnterPoint.transform.position != universalRoadBoundary.transform.position
                    && road.MergePointHolder.ExitPoint.transform.position != universalRoadBoundary.transform.position)
                || universalRoadBoundary.RoadBoundaryType != RoadBoundaryType.Start
                || road.IsConnect
                || !_chains.TryAddRoadInChain(universalRoadBoundary, road))
                continue;

            ConnectRoad(road, null, 0);

            return;
        }
    }

    private void ProcessPointCheck(Cell cell, RoadNode roadNode)
    {
        if (cell.TryGetRoad(out RoadNode findRoadNode))
        {
            if (roadNode.IsConnect == findRoadNode.IsConnect)
                return;

            if (!CanConnectRoad(findRoadNode.MergePointHolder.EnterPoint, roadNode)
                && !CanConnectRoad(findRoadNode.MergePointHolder.ExitPoint, roadNode))
                return;

            if (roadNode.IsConnect)
                ConnectRoad(findRoadNode, roadNode, roadNode.Index);
            else
                ConnectRoad(roadNode, findRoadNode, findRoadNode.Index);

            return;
        }

        if (!cell.TryGetRoadBoundary(out RoadBoundary roadBoundary)) return;

        if (roadBoundary is StartRoad startRoad)
        {
            AttemptChainCreationOnOccupiedPosition(roadNode, startRoad, startRoad.MergePoint);
            return;
        }
        
        if (roadBoundary is FinishRoad finishRoad)
        {
            if (roadBoundary.Index == finishRoad.Index
                && !finishRoad.IsConnect
                && roadNode.IsConnect
                && CanConnectRoad(finishRoad.MergePoint, roadNode))
            {
                finishRoad.Connect();


                return;
            }
        }

        if (roadBoundary is not UniversalRoadBoundary universalRoadNode
            || universalRoadNode.RoadBoundaryType != RoadBoundaryType.Finish
            || universalRoadNode.IsBroken
            || universalRoadNode.IsConnect
            || !roadNode.IsConnect)
            return;

        foreach (MergePoint mergePoint in universalRoadNode.MergePoints)
        {
            if (!CanConnectRoad(mergePoint, roadNode)) continue;

            universalRoadNode.Connect();

            return;
        }
    }

    private void AttemptChainCreationOnOccupiedPosition(RoadNode roadNode, StartRoad startRoad,
        MergePoint startMergePoint)
    {
        if (CanConnectRoad(startMergePoint, roadNode) && _chains.TryAddRoadInChain(startRoad, roadNode))
            ConnectRoad(roadNode, null, startRoad.Index);
    }

    private void ConnectRoad(RoadNode roadNodeToConnect, RoadNode findRoadNode, int index)
    {
        roadNodeToConnect.Connect(findRoadNode, index);
        findRoadNode?.OnConnect(roadNodeToConnect);
        TryBuildRoad(roadNodeToConnect);
    }

    private bool CanConnectRoad(MergePoint mergePoint, RoadNode currentRoadNode)
    {
        Cell cell = _grid.GetCell(mergePoint.transform.position);

        return cell != null
               && cell.TryGetRoad(out RoadNode road)
               && road != null
               && road == currentRoadNode;
    }
}
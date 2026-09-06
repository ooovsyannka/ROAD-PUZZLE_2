/*using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private Chains _chains;
    [SerializeField] private Grid _grid;

    private List<StartRoad> _startRoads;
    private List<UniversalRoadNode> _universalRoadNodes;
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
        */ /*        _startRoads = iStartRoads;
                _roadDrager.DragOver += TestStart;*/ /*
    }

    public void SetFinishRoads(List<FinishRoad> finishRoads)
    {
        _finishRoads = finishRoads;
    }

    public void SetUniversalRoadNode(List<UniversalRoadNode> universalRoadNode)
    {
        _universalRoadNodes = universalRoadNode;
        _roadDrager.DragOver += TryFindNearRoad;
    }

    private void TryBuildRoad(Road road)
    {
        Cell cell;

        if (_grid.TryGetCell(road.MergePointHolder.EnterPoint.transform.position, out cell))
        {
            ProcessPointCheck(cell, road);
        }

        if (_grid.TryGetCell(road.MergePointHolder.ExitPoint.transform.position, out cell))
        {
            ProcessPointCheck(cell, road);
        }
    }

    private void TryFindNearRoad(Road _)
    {
        bool isCorrectRoad;

        foreach (UniversalRoadNode universalRoadNode in _universalRoadNodes)
        {
            foreach (MergePoint mergePoint in universalRoadNode.MergePoints)
            {
                if (_grid.TryGetCell(mergePoint.transform.position, out Cell cell))
                {
                    if (cell.TryGetRoad(out Road road))
                    {
                        if (road.MergePointHolder.EnterPoint.transform.position == universalRoadNode.transform.position)
                        {
                            isCorrectRoad = true;
                        }
                        else if (road.MergePointHolder.ExitPoint.transform.position == universalRoadNode.transform.position)
                        {
                            isCorrectRoad = true;
                        }
                        else
                        {
                            isCorrectRoad = false;
                        }

                        if (isCorrectRoad)
                        {
                            if (universalRoadNode.RoadNodeType == RoadNodeType.Start)
                            {
                                if (road.IsConnect == false)
                                {
                                    if (_chains.TryAddRoadInChain(universalRoadNode, road))
                                    {
                                        ConnectRoad(road, null, 0);

                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ProcessPointCheck(Cell cell, Road road)
    {
        if (cell.TryGetRoad(out Road findRoad))
        {
            if (road.IsConnect == false)
            {
                if (findRoad.IsConnect)
                {
                    if (CanConnectedRoad(findRoad.MergePointHolder.EnterPoint, road))
                    {
                        ConnectRoad(road, findRoad, findRoad.Index);

                        return;
                    }
                    else if (CanConnectedRoad(findRoad.MergePointHolder.ExitPoint, road))
                    {
                        ConnectRoad(road, findRoad, findRoad.Index);

                        return;
                    }
                }
            }
            else
            {
                if (findRoad.IsConnect == false)
                {
                    if (CanConnectedRoad(findRoad.MergePointHolder.EnterPoint, road))
                    {
                        ConnectRoad(findRoad, road, road.Index);

                        return;
                    }
                    else if (CanConnectedRoad(findRoad.MergePointHolder.ExitPoint, road))
                    {
                        ConnectRoad(findRoad, road, road.Index);

                        return;
                    }
                }
            }
        }
        else if (cell.TryGetRoadNode(out Road roadNode))
        {
            if (roadNode is StartRoad startRoad)
            {
                AttemptChainCreationOnOccupiedPosition(road, startRoad, startRoad.MergePoint);
            }
            else if (roadNode is FinishRoad finishRoad)
            {
                if (road.Index == finishRoad.Index)
                {
                    if (finishRoad.IsConnect == false)
                    {
                        if (road.IsConnect)
                        {
                            if (IsPositionOccupiedByRoad(finishRoad.MergePoint.transform.position, road))
                            {
                                finishRoad.Connect();
                            }
                        }
                    }
                }
            }
            else if (roadNode is UniversalRoadNode universalRoadNode)
            {
                if (universalRoadNode.RoadNodeType == RoadNodeType.Finish)
                {
                    if (universalRoadNode.IsBroken == false)
                    {
                        if (universalRoadNode.IsConnect == false)
                        {
                            if (road.IsConnect)
                            {
                                foreach (MergePoint mergePoint in universalRoadNode.MergePoints)
                                {
                                    if (IsPositionOccupiedByRoad(mergePoint.transform.position, road))
                                    {
                                        universalRoadNode.Connect();

                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void AttemptChainCreationOnOccupiedPosition(Road road, StartRoad startRoad, MergePoint startMergePoint)
    {
        if (IsPositionOccupiedByRoad(startMergePoint.transform.position, road))
        {
            if (_chains.TryAddRoadInChain(startRoad, road))
            {
                ConnectRoad(road, null, startRoad.Index);
            }
        }
    }

    private void ConnectRoad(Road roadToConnect, Road findRoad, int index)
    {
        roadToConnect.Connect(findRoad, index);

        if (findRoad != null)
            findRoad.OnConnect(roadToConnect);

        TryBuildRoad(roadToConnect);
    }

    private bool CanConnectedRoad(MergePoint mergePoint, Road currentRoad)
    {
        if (IsPositionOccupiedByRoad(mergePoint.transform.position, currentRoad))
        {
            return true;
        }

        return false;
    }

    private bool IsPositionOccupiedByRoad(Vector3 mergePointPosition, Road currentRoad)
    {
        if (_grid.TryGetCell(mergePointPosition, out Cell cell))
        {
            if (cell.TryGetRoad(out Road road))
            {
                if (road != null)
                {
                    if (road == currentRoad)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

*/


using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private Chains _chains;
    [SerializeField] private Grid _grid;

    private List<StartRoad> _startRoads;
    private List<UniversalRoadBoundary> _universalRoadNodes;
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
        _universalRoadNodes = universalRoadNode;
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
        foreach (UniversalRoadBoundary universalRoadNode in _universalRoadNodes)
        foreach (MergePoint mergePoint in universalRoadNode.MergePoints)
        {
            if (!_grid.TryGetCell(mergePoint.transform.position, out Cell cell)
                || !cell.TryGetRoad(out RoadNode road)
                || (road.MergePointHolder.EnterPoint.transform.position != universalRoadNode.transform.position
                    && road.MergePointHolder.ExitPoint.transform.position != universalRoadNode.transform.position)
                || universalRoadNode.RoadBoundaryType != RoadBoundaryType.Start
                || road.IsConnect
                || !_chains.TryAddRoadInChain(universalRoadNode, road))
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

        if (!cell.TryGetRoadNode(out RoadBoundary roadBoundary)) return;

        if (roadBoundary is StartRoad startRoad)
        {
            AttemptChainCreationOnOccupiedPosition(roadNode, startRoad, startRoad.MergePoint);
            return;
        }

        if (roadBoundary is FinishRoad finishRoad)
        {
            if (roadBoundary.Index == finishRoad.Index
                && !finishRoad.IsConnect
                && roadBoundary.IsConnect
                && CanConnectRoad(finishRoad.MergePoint, roadNode))
                finishRoad.Connect();

            return;
        }

        if (roadBoundary is not UniversalRoadBoundary universalRoadNode
            || universalRoadNode.RoadBoundaryType != RoadBoundaryType.Finish
            || universalRoadNode.IsBroken
            || universalRoadNode.IsConnect
            || !roadBoundary.IsConnect)
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
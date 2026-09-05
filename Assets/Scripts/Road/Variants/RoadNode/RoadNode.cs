using UnityEngine;
using System;
using UnityEngine.Serialization;

public class RoadNode : MonoBehaviour
{
    [SerializeField] private RoadMover _mover;
    [FormerlySerializedAs("_roadType")] [SerializeField] private RoadNodeType roadNodeType;
    [SerializeField] private RoadPreview _preview;
    [SerializeField] private MergePointHolder _mergePointHolder;
    [SerializeField] private SingleRoadHolder _singleRoadHolder;
    [SerializeField] private RoadRotation _roadRotation;

    private int _index;
    private bool _canDrag;
    public bool _isConnect;

    public SingleRoadHolder SingleRoadHolder => _singleRoadHolder;
    public MergePointHolder MergePointHolder => _mergePointHolder;
    public RoadRotation RoadRotation => _roadRotation;
    public RoadPreview Preview => _preview;
    public RoadNodeType RoadNodeType => roadNodeType;
    public bool IsConnect => _isConnect;
    public bool CanDrag => _canDrag;    
    public int Index => _index;

    public event Action<RoadNode> Disconected;
    public event Action<RoadNode, RoadNode> RoadJoined;


    private void OnEnable()
    {
        _canDrag = true;
    }

    public void StopDrag()
    {
        _canDrag = false;
    }

    public void Disconnect(RoadNode roadNode)
    {
        _isConnect = false;
        Disconected?.Invoke(this);
        _singleRoadHolder.DisconnectSingleRoad();

        if (roadNode != null)
        {
            roadNode.Disconected -= Disconnect;
        }
    }

    public void Connect(RoadNode attachedRoadNode, int index)
    {
        _isConnect = true;
        _singleRoadHolder.ConnectSingleRoad();
        _index = index;

        if (attachedRoadNode != null)
        {
            attachedRoadNode.Disconected += Disconnect;
        }
    }

    public void OnConnect(RoadNode attachedRoadNode)
    {
        RoadJoined?.Invoke(this, attachedRoadNode);
    }

    public void Move(Vector3 point)
    {
        _mover.Move(point);
    }

    public void AddRoad(SingleRoad singleRoad)
    {
        _singleRoadHolder.AddSingleRoad(singleRoad);
    }

    public void AddMergePoint(MergePoint enterPoint, MergePoint exitPoint)
    {
        _mergePointHolder.SetMergePoint(enterPoint, exitPoint);
    }

    public void SetRoadPreview(SinglePreview preview)
    {
        preview.transform.SetParent(_preview.transform);
    }
}

using UnityEngine;
using System;

public class Road : MonoBehaviour
{
    [SerializeField] private RoadMover _mover;
    [SerializeField] private RoadType _roadType;
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
    public RoadType RoadType => _roadType;
    public bool IsConnect => _isConnect;
    public bool CanDrag => _canDrag;    
    public int Index => _index;

    public event Action<Road> Disconected;
    public event Action<Road, Road> RoadJoined;


    private void OnEnable()
    {
        _canDrag = true;
    }

    public void StopDrag()
    {
        _canDrag = false;
    }

    public void Disconnect(Road road)
    {
        _isConnect = false;
        Disconected?.Invoke(this);
        _singleRoadHolder.DisconnectSingleRoad();

        if (road != null)
        {
            road.Disconected -= Disconnect;
        }
    }

    public void Connect(Road attachedRoad, int index)
    {
        _isConnect = true;
        _singleRoadHolder.ConnectSingleRoad();
        _index = index;

        if (attachedRoad != null)
        {
            attachedRoad.Disconected += Disconnect;
        }
    }

    public void OnConnect(Road attachedRoad)
    {
        RoadJoined?.Invoke(this, attachedRoad);
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class UniversalRoadNode : RoadNode, IStartRoad, IFinishRoad
{
    [SerializeField] private List<MergePoint> _mergePoint;
    [SerializeField] private List<RoadRender> _roadRenders;
    [SerializeField] private RoadNodeType _roadNodeType;
    [SerializeField] private SparkPraticle _sparkPraticle;
    [SerializeField] private UniversalRoadNodeAnimation _animation;

    private bool _isBroken;
    private Car _car;

    public Car Car => _car;
    public RoadNodeType RoadNodeType => _roadNodeType;
    public List<MergePoint> MergePoints => _mergePoint;
    public bool IsBroken => _isBroken;

    public event Action<IFinishRoad> OnConnected;

    private void OnEnable()
    {
        Disconnect();
    }

    public void Connect()
    {
        foreach (RoadRender roadRender in _roadRenders)
        {
            roadRender.SetConnectRender();
        }
        _isConnect = true;
        OnConnected?.Invoke(this);
    }

    public void SetCar(Car car)
    {
        _car = car;
        SetStarType();
    }

    public void SetStarType()
    {
        foreach (RoadRender roadRender in _roadRenders)
        {
            roadRender.SetConnectRender();
        }

        Fix();
        _roadNodeType = RoadNodeType.Start;
    }

    public void SetFinishType()
    {
        _roadNodeType = RoadNodeType.Finish;

        foreach (RoadRender roadRender in _roadRenders)
        {
            roadRender.SetDisconnectRender();
        }

        _isConnect = false;
    }

    public void Broken()
    {
        if (_roadNodeType == RoadNodeType.Finish)
        {
            _isBroken = true;
            _sparkPraticle.TurnOn();

            foreach (RoadRender roadRender in _roadRenders)
            {
                roadRender.SetDarkRenderSideWalk();
            }

            _animation.PlayAnimation();
        }
    }

    public void Fix()
    {
        if (_roadNodeType == RoadNodeType.Finish)
        {
            _isBroken = false;
            _sparkPraticle.TurnOff();

            foreach (RoadRender roadRender in _roadRenders)
            {
                roadRender.SetDisconnectRender();
            }
        }
            _animation.StopAnimation();
    }

    public void Disconnect()
    {
        foreach (RoadRender roadRender in _roadRenders)
        {
            roadRender.SetDisconnectRender();
        }
        _isConnect = false;
    }
}

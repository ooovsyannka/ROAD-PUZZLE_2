using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UniversalRoadBoundary : RoadBoundary, IStartRoad, IFinishRoad
{
    [SerializeField] private List<MergePoint> _mergePoint;
    [SerializeField] private List<RoadRender> _roadRenders;
    [FormerlySerializedAs("_roadNodeType")] [SerializeField] private RoadBoundaryType roadBoundaryType;
    [SerializeField] private SparkPraticle _sparkPraticle;
    [SerializeField] private UniversalRoadNodeAnimation _animation;

    private bool _isBroken;
    private Car _car;

    public Car Car => _car;
    public RoadBoundaryType RoadBoundaryType => roadBoundaryType;
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
        roadBoundaryType = RoadBoundaryType.Start;
    }

    public void SetFinishType()
    {
        roadBoundaryType = RoadBoundaryType.Finish;

        foreach (RoadRender roadRender in _roadRenders)
        {
            roadRender.SetDisconnectRender();
        }

        _isConnect = false;
    }

    public void Broken()
    {
        if (roadBoundaryType == RoadBoundaryType.Finish)
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
        if (roadBoundaryType == RoadBoundaryType.Finish)
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

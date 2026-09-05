using System;
using System.Collections;
using UnityEngine;

public class FinishRoad : RoadBoundary, IFinishRoad
{
    [SerializeField] private MergePoint _mergePoint;
    [SerializeField] private RoadRender _roadRender;

    public MergePoint MergePoint => _mergePoint;

    public event Action<IFinishRoad> OnConnected;


    private void OnEnable()
    {
        Disconnect();
    }

    public void Connect()
    {
        _roadRender.SetConnectRender();
        _isConnect = true;
        OnConnected?.Invoke(this);
    }

    public void Disconnect()
    {
        _isConnect = false;
        _roadRender.SetDisconnectRender();
    }
}
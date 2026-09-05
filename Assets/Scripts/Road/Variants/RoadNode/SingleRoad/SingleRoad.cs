using Dreamteck.Splines;
using UnityEngine;

public class SingleRoad : MonoBehaviour
{
    [SerializeField] private RoadRender _roadRender;
    [SerializeField] private SplineComputer _splineComputer;

    public SplineComputer SplineComputer => _splineComputer;

    public  void Disconnect()
    {
        _roadRender.SetDisconnectRender();
    }

    public  void Connect()
    {
        _roadRender.SetConnectRender();
    }
}
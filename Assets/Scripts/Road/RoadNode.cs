using UnityEngine;
using Dreamteck.Splines;

public abstract class RoadNode : MonoBehaviour
{
    [SerializeField] private SplineComputer _splineComputer;

    protected bool _isConnect;

    private int _index;

    public  bool IsConnect => _isConnect;
    public int Index => _index;
    public SplineComputer SplineComputer => _splineComputer;

    public void SetIndex(int index)
    {
        _index = index;
    }
}

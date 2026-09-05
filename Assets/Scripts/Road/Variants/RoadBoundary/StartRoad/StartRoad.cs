using UnityEngine;

public class StartRoad : RoadBoundary, IStartRoad
{
    [SerializeField] private MergePoint _mergePoint;
    [SerializeField] private Car _car;
    
    public MergePoint MergePoint => _mergePoint;
    public Car Car  => _car;

    public void SetCar(Car car)
    {
        _car = car; 
    }
}

using Dreamteck.Splines;
using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private CarMover _carMover;
    [SerializeField] private Headlights _headlights;

    public  int Speed => _carMover.Speed;
    public event Action<Car> MoveFinished;
    public event Action<Car> Died;

    private void OnEnable()
    {
        if (_carMover != null)
            _carMover.MoveFinished += MoveFinish;
    }


    private void OnDisable()
    {
        if (_carMover != null)
            _carMover.MoveFinished -= MoveFinish;
    }

    public void Move(SplineComputer route, SplineComputer finishSpline)
    {
        _carMover.Move(route, finishSpline);
    }

    private void MoveFinish()
    {
        MoveFinished?.Invoke(this);
    }
    
    public void UpdateHeadlightsBasedOnTime(TimeOfDay timeOfDay)
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Morning: _headlights.TurnOff();
                break;
            case TimeOfDay.Evening: _headlights.TurnOn();
                break;
            case TimeOfDay.Night: _headlights.TurnOn();
                break;
        }
    }

    public void Die()
    {
        Died?.Invoke(this);
    }
}
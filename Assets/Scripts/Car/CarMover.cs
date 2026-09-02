using Dreamteck.Splines;
using System;
using System.Collections;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    [SerializeField] private int _speed = 10;
    [SerializeField] private SplineFollower _splineFollower;
    private float _minDistance = 7.5f;

    private Coroutine _addSpeed;
    private Coroutine _minusSpeed;
    private Coroutine _waitForSplineReachAndStartMinusSpeed;
    private float _speedMultiplier = 0.5f;
    private float _currentSpeed = 0;

    public int Speed => _speed;
    public event Action MoveFinished;

    public void Move(SplineComputer route, SplineComputer finishSpline)
    {
        _splineFollower.spline = route;
        _splineFollower.follow = true;

        if (_addSpeed != null)
            StopCoroutine(_addSpeed);

        _addSpeed = StartCoroutine(AddSpeed());

        if (_waitForSplineReachAndStartMinusSpeed != null)
            StopCoroutine(_waitForSplineReachAndStartMinusSpeed);

        _waitForSplineReachAndStartMinusSpeed = StartCoroutine(WaitForSplineReachAndStartMinusSpeed(finishSpline));
    }

    private IEnumerator AddSpeed()
    {
        while (_currentSpeed < _speed)
        {
            _currentSpeed += _speedMultiplier;
            _splineFollower.followSpeed = _currentSpeed;

            yield return null;
        }

        _splineFollower.followSpeed = _currentSpeed;
    }

    private IEnumerator WaitForSplineReachAndStartMinusSpeed(SplineComputer finishSpline)
    {
        while (Vector3Extensions.IsEnoughClose(transform.position, finishSpline.transform.position, _minDistance) == false)
        {
            yield return null;
        }

        if (_minusSpeed != null)
            StopCoroutine(_minusSpeed);

        _minusSpeed = StartCoroutine(MinusSpeed());
    }

    private IEnumerator MinusSpeed()
    {
        while (_currentSpeed > 0)
        {
            _currentSpeed -= _speedMultiplier;
            _splineFollower.followSpeed = _currentSpeed;

            yield return null;
        }

        _splineFollower.Restart();
        MoveFinished?.Invoke();
    }
}

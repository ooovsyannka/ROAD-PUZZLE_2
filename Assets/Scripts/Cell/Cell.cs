using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellAnimation _animation;

    private Road _road;
    private RoadNode _roadNode;

    public bool _isFree;

    public bool IsFree => _isFree;
    public int Index;

    public Vector3 PositionInCell;

    public event Action<Cell> AnimationFinished;
    public event Action Filled;
    public event Action BecameEmpty;

    public Road Road => _road;

    private void Awake()
    {
        if (_roadNode != null)
        {
            Fill();
        }
        else
        {
            EmptyCell();
        }
    }

    private void OnEnable()
    {
        if (_animation != null)
            _animation.Finished += AnimationFinish;

    }

    private void OnDisable()
    {
        if (_animation != null)
            _animation.Finished -= AnimationFinish;
    }

    public bool TryGetRoad(out Road road)
    {
        road = null;

        if (_road != null)
        {
            road = _road;

            return true;
        }

        return false;
    }

    public void CleanRoad()
    {
        _road.transform.SetParent(null);
        _road = null;
        EmptyCell();
    }

    public bool TryGetRoadNode(out RoadNode roadNode)
    {
        roadNode = null;

        if (_roadNode != null)
        {
            roadNode = _roadNode;

            return true;
        }

        return false;
    }

    public void SetRoad(Road road)
    {
        _road = road;
        Fill();

        if (_animation != null)
           _road.transform.SetParent(_animation.transform);
    }

    public void SetRoadNode(RoadNode roadNode)
    {
        _roadNode = roadNode;
        Fill();
    }

    public void PlayAnimation()
    {
        _animation.PlayAnimation();
    }

    public void Fill()
    {
        _isFree = false;
        Filled?.Invoke();
    }

    public void EmptyCell()
    {
        _isFree = true;
        BecameEmpty?.Invoke();
    }

    private void AnimationFinish()
    {
        AnimationFinished?.Invoke(this);
    }

}


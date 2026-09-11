 using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellAnimation _animation;

    private RoadNode _roadNode;
    private RoadBoundary _roadBoundary;
    private bool _isFree;

    public bool IsFree => _isFree;
    public int Index;

    public Vector3 PositionInCell;

    public event Action<Cell> AnimationFinished;
    public event Action Filled;
    public event Action BecameEmpty;

    public RoadNode RoadNode => _roadNode;

    private void Awake()
    {
        if (_roadBoundary != null)
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

    public bool TryGetRoad(out RoadNode roadNode)
    {
        roadNode = null;

        if (_roadNode != null)
        {
            roadNode = _roadNode;

            return true;
        }

        return false;
    }

    public void CleanRoad()
    {
        _roadNode.transform.SetParent(null);
        _roadNode = null;
        EmptyCell();
    }

    public bool TryGetRoadBoundary(out RoadBoundary roadBoundary)
    {
        roadBoundary = null;

        if (_roadBoundary != null)
        {
            roadBoundary = _roadBoundary;

            return true;
        }

        return false;
    }

    public void SetRoad(RoadNode roadNode)
    {
        _roadNode = roadNode;
        Fill();

        if (_animation != null)
           _roadNode.transform.SetParent(_animation.transform);
    }

    public void SetRoadBoundary(RoadBoundary roadBoundary)
    {
        _roadBoundary = roadBoundary;
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


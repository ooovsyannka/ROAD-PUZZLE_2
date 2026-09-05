using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDrager : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private Grid _grid;

    private Cell _lastCell;
    private RoadNode _roadNode;
    private RoadPreview _preview;
    private Vector3 _hitPosition;
    private Coroutine _drag;
    private RaycastHit _hit;
    private bool _isDrag;

    public event Action<RoadNode> DragOver;

    private void OnEnable()
    {
        _reader.RoadPickupAttempt += TryGetRoad;
        _reader.RoadDropAttempt += TryDragOver;
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    private void TryGetRoad()
    {
        if (_reader.IsCorrectPosition(out _hit))
        {
            if (_hit.transform.TryGetComponent(out Cell cell))
            {
                if (cell.TryGetRoad(out _roadNode))
                {
                    if (_roadNode.CanDrag)
                    {
                        _lastCell = cell;
                        _preview = _roadNode.Preview;
                        _preview.SetParent(null);

                        if (_roadNode.IsConnect)
                        {
                            _roadNode.Disconnect(null);
                        }

                        if (_roadNode.SingleRoadHolder.SingleRoads.Count > 1)
                        {
                            foreach (SingleRoad singleRoad in _roadNode.SingleRoadHolder.SingleRoads)
                            {
                                CleanCells(singleRoad.transform.position);
                            }
                        }
                        else
                        {
                            cell.CleanRoad();
                        }

                        if (_drag != null)
                            StopCoroutine(_drag);

                        _drag = StartCoroutine(Drag());
                    }
                }
            }
        }
    }

    private void TryDragOver()
    {
        _isDrag = false;
    }

    private void CleanCells(Vector3 point)
    {
        if (_grid.TryGetCell(point, out Cell cell))
        {
            cell.CleanRoad();
        }
    }

    private void DropRoad()
    {
        bool canDropRoadInCell = false;

        foreach (SingleRoad singleRoad in _roadNode.SingleRoadHolder.SingleRoads)
        {
            if (_grid.TryGetCell(_preview.transform.position + singleRoad.transform.localPosition, out Cell cell))
            {
                canDropRoadInCell = true;
                cell.SetRoad(_roadNode);
            }
        }

        if (canDropRoadInCell == false)
        {
            _lastCell.SetRoad(_roadNode);
        }

        _roadNode.transform.position = _preview.transform.position;
        _preview.SetParent(_roadNode.transform);
    }

    private void ClearTemporaryData()
    {
        _roadNode = null;
        _preview = null;
    }

    private IEnumerator Drag()
    {
        _isDrag = true;
        bool canShowPreviewPosition;
        Vector3 previewPosition;
        List<SingleRoad> roads = _roadNode.SingleRoadHolder.SingleRoads;

        while (_isDrag)
        {
            Cell cell;

            if (_reader.IsCorrectPosition(out _hit))
            {
                _roadNode.Move(_hit.point);
                _roadNode.RoadRotation.SetRotation(_hit.point);

                if (_grid.TryGetCell(_hit.point, out _))
                {
                    _hitPosition = _hit.transform.position + _hit.normal;
                    canShowPreviewPosition = true;
                    previewPosition = _hitPosition;

                    foreach (SingleRoad singleRoad in roads)
                    {
                        if (_grid.TryGetCell(previewPosition + singleRoad.transform.localPosition, out cell))
                        {
                            if (cell.IsFree == false)
                            {
                                if (_grid.TryGetCell(previewPosition - singleRoad.transform.localPosition, out cell))
                                {
                                    if (cell.IsFree)
                                    {
                                        previewPosition = previewPosition - singleRoad.transform.localPosition;
                                    }
                                }
                            }
                        }
                        else if (_grid.TryGetCell(previewPosition - singleRoad.transform.localPosition, out cell))
                        {
                            if (cell.IsFree)
                            {
                                previewPosition = previewPosition - singleRoad.transform.localPosition;
                            }
                        }
                    }

                    foreach (SingleRoad singleRoad in roads)
                    {
                        if (_grid.TryGetCell(previewPosition + singleRoad.transform.localPosition, out cell))
                        {
                            if (cell.IsFree == false)
                            {
                                canShowPreviewPosition = false;

                                break;
                            }
                        }
                        else
                        {
                            canShowPreviewPosition = false;

                            break;
                        }
                    }

                    if (canShowPreviewPosition)
                    {
                        _preview.ShowPreviwPosition(previewPosition);
                    }
                }
                else
                {
                    _preview.ShowPreviwPosition(_lastCell.transform.position);
                }
            }

            yield return null;
        }

        _roadNode.RoadRotation.ResetTiltImmediately();
        DropRoad();

        yield return new WaitForSeconds(0.05f);

        DragOver.Invoke(_roadNode);
        ClearTemporaryData();
    }
}
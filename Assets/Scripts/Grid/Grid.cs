using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<Cell> _cells;

    private int _maxLengthGridY = 5;
    private int _maxLengthGridX = 5;
    private Cell[,] _gridCells;
    private int _fillCellCount;

    public bool IsFull => _fillCellCount != _cells.Count;
    public List<Cell> Cells => _cells;

    private void OnDisable()
    {
        foreach (Cell cell in _cells)
        {
            cell.BecameEmpty -= MinusFreeCellCount;
            cell.Filled -= AddFreeCellCount;
        }
    }

    public void InitializeGrid()
    {
        if (_cells != null)
        {
            _gridCells = new Cell[_maxLengthGridY, _maxLengthGridX];
            int index = 0;

            for (int y = 0; y < _maxLengthGridY; y++)
            {
                for (int x = 0; x < _maxLengthGridX; x++)
                {
                    Cell cell = _cells[index];
                    cell.Index = index;
                    cell.BecameEmpty += MinusFreeCellCount;
                    cell.Filled += AddFreeCellCount;
                    _gridCells[y, x] = cell;
                    index++;
                }
            }
        }
    }

    public void SetGridInfo(List<Cell> cells, int maxLengthGridY, int maxLengthGridX)
    {
        _cells = cells;
        _maxLengthGridX = maxLengthGridX;
        _maxLengthGridY = maxLengthGridY;
        InitializeGrid();
    }

    public Cell GetCell(Vector3 cellPosition)
    {
        Vector3 index = cellPosition
            .RoundVector3ToNearest()
            .ConvertVectorToIndex();

        return GetCellByIndex(index);
    }

    public bool HasCell(Vector3 cellPosition)
    {
        return GetCell(cellPosition) != null;
    }

    public bool HasEmptyCell(Vector3 cellPosition)
    {
        Cell cell = GetCell(cellPosition);

        return cell != null && cell.IsFree;
    }

    public void FindValidPreviewPosition(List<SingleRoad> roads, ref Vector3 previewPosition)
    {
        foreach (SingleRoad singleRoad in roads)
        {
            if (HasEmptyCell(previewPosition + singleRoad.transform.localPosition) == false)
            {
                if (HasEmptyCell(previewPosition - singleRoad.transform.localPosition))
                {
                    previewPosition -= singleRoad.transform.localPosition;
                }
            }
        }
    }

    public bool IsValidPreviewPosition(List<SingleRoad> roads, Vector3 previewPosition)
    {
        foreach (SingleRoad singleRoad in roads)
        {
            if (HasEmptyCell(previewPosition + singleRoad.transform.localPosition) == false)
            {
                return false;
            }
        }

        return true;
    }

    public bool TryGetCell(Vector3 cellPosition, out Cell cell) =>
        TryGetCellByIndex(cellPosition.RoundVector3ToNearest().ConvertVectorToIndex(), out cell);

    public Cell TryGetRandomEmptyCell()
    {
        Cell cell = null;

        if (IsFull)
        {
            bool _isCreateCell = false;

            while (_isCreateCell == false)
            {
                int randomCellIndex = Random.Range(0, _cells.Count);
                cell = _cells[randomCellIndex];

                if (cell.IsFree)
                {
                    _isCreateCell = true;
                }
            }
        }

        return cell;
    }

    private bool TryGetCellByIndex(Vector3 positionInGrind, out Cell cell)
    {
        cell = null;

        if (positionInGrind.x < 0 || positionInGrind.x >= _maxLengthGridX)
            return false;
        if (positionInGrind.z < 0 || positionInGrind.z >= _maxLengthGridY)
            return false;

        Cell cellInGrind;

        if (!(positionInGrind.x < _maxLengthGridX) || !(positionInGrind.z < _maxLengthGridY)) return false;
        cellInGrind = _gridCells[(int)positionInGrind.z, (int)positionInGrind.x];

        if (cellInGrind == null) return false;
        cell = cellInGrind;

        return true;
    }

    private Cell GetCellByIndex(Vector3 positionInGrid)
    {
        if (!IsInsideGrid(positionInGrid))
            return null;

        return _gridCells[(int)positionInGrid.z, (int)positionInGrid.x];
    }

    private bool IsInsideGrid(Vector3 positionInGrid)
    {
        return positionInGrid.x >= 0 &&
               positionInGrid.x < _maxLengthGridX &&
               positionInGrid.z >= 0 &&
               positionInGrid.z < _maxLengthGridY;
    }

    private void AddFreeCellCount()
    {
        _fillCellCount++;
    }

    private void MinusFreeCellCount()
    {
        _fillCellCount--;
    }
}
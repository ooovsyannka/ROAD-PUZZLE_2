using UnityEngine;
using System.Collections.Generic;

public class GridInstantiator : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Grid _gridPrefab;

    public Grid InstantiateGrid(GridSection gridSection)
    {
        Grid grid = Instantiate(_gridPrefab);
        List<Cell> cells = new List<Cell>();
        Vector3 cellPosition = Vector3.down;
        int index = 0;

        for (int y = 0; y < gridSection.MaxLengthY; y++)
        {
            for (int x = 0; x < gridSection.MaxLengthX; x++)
            {
                index++;
                cellPosition = new Vector3(x * 15, cellPosition.y, y * 15);
                Cell cell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, grid.transform);
                cell.Index = index; 
                cells.Add(cell);
            }
        }

        grid.SetGridInfo(cells, gridSection.MaxLengthY, gridSection.MaxLengthX);

        return grid;
    }
}

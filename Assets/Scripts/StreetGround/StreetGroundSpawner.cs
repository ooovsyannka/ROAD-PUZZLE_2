using System.Collections.Generic;
using UnityEngine;

public class StreetGroundSpawner : MonoBehaviour
{
    [SerializeField] private StreetGround _streetGround;
    [SerializeField] private Grid _grid;

    private List<StreetGround> _tempStreetGrounds;
    private Spawner<StreetGround> _streetGroundSpawner;
    private int _streetGroundCount = 3;

    private void Awake()
    {
        _streetGroundSpawner = new Spawner<StreetGround>(_streetGround);
        _tempStreetGrounds = new List<StreetGround>();
    }

    public void SpawnStreetGround()
    {
        for (int i = 0; i < _streetGroundCount; i++)
        {
            Cell cell = _grid.TryGetRandomEmptyCell();

            if (cell != null)
            {
                cell.Fill();
                StreetGround streetGround = _streetGroundSpawner.Spawn(cell.transform.position + Vector3.up, null);
                streetGround.Died += _streetGroundSpawner.ReturnObjectInPool;
                _tempStreetGrounds.Add(streetGround);
                streetGround.RotatoinStretLampByCell(_grid.Cells[12]);//*

                if (streetGround.isActiveAndEnabled)
                {
                    streetGround.PlayGrowAnimation();
                }
            }
        }
    }

    public void DespawnStreetGround()
    {
        foreach (StreetGround streetGround in _tempStreetGrounds)
        {
            streetGround.PlaySmallerAnimation();
            streetGround.Died += ReturnStreetGroundInPool;
            _grid.TryGetCell(streetGround.transform.position, out Cell cell);
            cell.EmptyCell();

        }
    }

    private void ReturnStreetGroundInPool(StreetGround streetGround)
    {
        streetGround.Died -= _streetGroundSpawner.ReturnObjectInPool;
    }
}
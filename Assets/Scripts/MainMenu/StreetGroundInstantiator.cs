using UnityEngine;
using System.Collections.Generic;

public class StreetGroundInstantiator : MonoBehaviour
{
    [SerializeField] private StreetGround _streetGroundPrefab;

    private List<StreetGround> _streetGrounds = new List<StreetGround>();

    public List<StreetGround> StreetGrounds => _streetGrounds;

    public void TryInstantiateStreetGround(List<TransformSection> transformSections, Grid grid)
    {
        if (transformSections != null)
        {
            foreach (TransformSection transformSection in transformSections)
            {
                StreetGround streetGround = Instantiate(_streetGroundPrefab, transformSection.Position, Quaternion.identity);
                streetGround.SetRotationStreetLamp(transformSection.Rotation);

                if (grid.TryGetCell(streetGround.transform.position, out Cell cell))
                {
                    cell.Fill();
                }

                _streetGrounds.Add(streetGround);
            }
        }
    }
}
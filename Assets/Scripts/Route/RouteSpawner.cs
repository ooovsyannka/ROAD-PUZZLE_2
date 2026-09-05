using UnityEngine;

public class RouteSpawner : MonoBehaviour
{
    [SerializeField] private Route _routePrefab;

    public Route InstantieteRoute() =>
        Instantiate(_routePrefab, Vector3.zero, Quaternion.identity, transform);

}
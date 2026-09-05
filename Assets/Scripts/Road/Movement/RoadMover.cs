using UnityEngine;

public class RoadMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _offsetY = 10;

    public void Move(Vector3 hitPoint)
    {
        Vector3 targetPosition = new Vector3(hitPoint.x, _offsetY, hitPoint.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
    }
}
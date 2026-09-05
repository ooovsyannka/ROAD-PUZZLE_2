using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    [SerializeField] private StreetLampRotation _rotation;

    public void LookAtTarget(Transform targetTransform)
    {
        _rotation.LookAtTarget(targetTransform);
    }

    public void SetRotation(Quaternion quaternion)
    {
        _rotation.SetRotation(quaternion);
    }
}

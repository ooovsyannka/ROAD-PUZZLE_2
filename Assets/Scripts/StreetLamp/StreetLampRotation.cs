using UnityEngine;

public class StreetLampRotation : MonoBehaviour
{
    public void LookAtTarget(Transform targetTransform)
    {
        Vector3 objectPosition = transform.position;
        Vector3 targetPosition = targetTransform.position;

        Vector3 directionToTarget = targetPosition - objectPosition;

        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        if (horizontalDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);

            transform.rotation = lookRotation;
        }
    }

    public void SetRotation(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }
}
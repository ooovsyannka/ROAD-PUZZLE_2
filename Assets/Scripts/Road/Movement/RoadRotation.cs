using UnityEngine;
using System.Collections;

public class RoadRotation : MonoBehaviour
{
    [SerializeField] private float _maxTiltAngle;

    private float _minDistance = 0.15f;
    private float _minAngel = 0.1f;
    private float _returnDuration = 0.3f;
    private Quaternion _initialLocalRotation;
    private Coroutine _returnCoroutine;

    public  void SetInitialRotation()
    {
        _initialLocalRotation = transform.localRotation;
    }

    public void SetRotation(Vector3 hitPoint)
    {
        Vector3 directionVector = hitPoint - transform.position;

        if (Vector3Extensions.IsEnoughClose(transform.position, hitPoint, _minDistance) == false)
        {
            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }

            Vector3 normalizedDirection = directionVector.normalized;

            float rotationX = normalizedDirection.z * _maxTiltAngle;
            float rotationZ = -normalizedDirection.x * _maxTiltAngle;

            rotationX = Mathf.Clamp(rotationX, -_maxTiltAngle, _maxTiltAngle);
            rotationZ = Mathf.Clamp(rotationZ, -_maxTiltAngle, _maxTiltAngle);


            Quaternion targetLocalRotation = Quaternion.Euler(rotationX, transform.rotation.y, rotationZ)* _initialLocalRotation;

            transform.localRotation = targetLocalRotation;
        }
        else
        {
            if (_returnCoroutine == null)
            {
                ReturnToInitialRotation();
            }
        }
    }

    public void ReturnToInitialRotation()
    {
        if (_returnCoroutine != null) return;

        if (Quaternion.Angle(transform.localRotation, _initialLocalRotation) < _minAngel)
        {
            transform.localRotation = _initialLocalRotation;

            return;
        }

        _returnCoroutine = StartCoroutine(SmoothReturn());
    }

    public void ResetTiltImmediately()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }

        transform.localRotation = _initialLocalRotation;
    }

    private IEnumerator SmoothReturn()
    {
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < _returnDuration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, _initialLocalRotation, elapsedTime / _returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = _initialLocalRotation;
        _returnCoroutine = null;
    }
}
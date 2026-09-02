using System.Collections;
using UnityEngine;

public class Headlights : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _durationChangeIntensity;
    private float _maxIntensity;

    private Coroutine _smothlyChangeIntensity;

    private void Awake()
    {
        _maxIntensity = _light.intensity;
    }

    public void TurnOn()
    {
        if (_smothlyChangeIntensity != null) 
            StopCoroutine( _smothlyChangeIntensity );

        _smothlyChangeIntensity = StartCoroutine( SmothlyChangeIntensity(_maxIntensity));
    }

    public void TurnOff()
    {
        if (_smothlyChangeIntensity != null)
            StopCoroutine(_smothlyChangeIntensity);

        _smothlyChangeIntensity = StartCoroutine(SmothlyChangeIntensity(0));
    }

    private IEnumerator SmothlyChangeIntensity(float desiredIntensity)
    {
        float time = 0;
        float startIntensity = _light.intensity;

        while (time < _durationChangeIntensity)
        {
            time += Time.deltaTime;
            float progress = Mathf.Clamp01(time / _durationChangeIntensity);
            _light.intensity = Mathf.RoundToInt(Mathf.Lerp(startIntensity, desiredIntensity, progress));

            yield return null;
        }

        _light.intensity = desiredIntensity;
    }
}

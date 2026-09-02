using System.Collections;
using UnityEngine;

public class LightCycle : MonoBehaviour
{
    private const int CountToMorning = 1;
    private const int CountToEvening = 3;
    private const int CountToNight = 4;
    private const int CountToUpdate = 7;

    [SerializeField] private Light _light;
    [SerializeField] private StreetGroundSpawner _streetGroundSpawner;
    [SerializeField] private float _delay;

    private float _morningIntensity = 1;
    private float _eveningLightIntensity = 0.4f;
    private float _nightIntensity = 0.15f;
    private TimeOfDay _timeOfDay;
    private int _currentCount = 0;
    private Coroutine _smothlyChangeLightIntensity;

    public TimeOfDay TimeOfDay => _timeOfDay;

    public void TryChangeLightIntensity()
    {
        _currentCount++;

        if (_currentCount == CountToMorning)
        {
            LaunchSmothlyChangeLightIntensity(_morningIntensity);
            _timeOfDay = TimeOfDay.Morning;
        }
        else if (_currentCount == CountToEvening)
        {
            LaunchSmothlyChangeLightIntensity(_eveningLightIntensity);
            _timeOfDay = TimeOfDay.Evening;
        }
        else if (_currentCount == CountToNight)
        {
            LaunchSmothlyChangeLightIntensity(_nightIntensity);
            _streetGroundSpawner.SpawnStreetGround();
            _timeOfDay = TimeOfDay.Night;
        }
        else if (_currentCount == CountToUpdate)
        {
            _currentCount = 0;
            _timeOfDay = TimeOfDay.Morning;
            LaunchSmothlyChangeLightIntensity(_morningIntensity);
            _streetGroundSpawner.DespawnStreetGround();
        }
    }

    public float GetLightIntensity(TimeOfDay timeOfDay)
    {
        float lightIntensity = 0f;

        switch (timeOfDay)
        {
            case TimeOfDay.Morning:
                lightIntensity = _morningIntensity;
                break;
            case TimeOfDay.Evening:
                lightIntensity = _eveningLightIntensity;
                break;
            case TimeOfDay.Night:
                lightIntensity = _nightIntensity;
                break;
        }

        return lightIntensity;
    }

    private void LaunchSmothlyChangeLightIntensity(float value)
    {
        if (_smothlyChangeLightIntensity != null)
            StopCoroutine(_smothlyChangeLightIntensity);

        _smothlyChangeLightIntensity = StartCoroutine(SmothlyChangeLightIntensity(value));
    }

    private IEnumerator SmothlyChangeLightIntensity(float value)
    {
        while (_light.intensity != value)
        {
            _light.intensity = Mathf.MoveTowards(_light.intensity, value, _delay * Time.deltaTime);

            yield return null;
        }
    }
}

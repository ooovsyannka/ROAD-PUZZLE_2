using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour
{
    private const int MaxSecondInMinute = 60;

    [SerializeField] private TimerRender _timerRender;

    private float _maxTimeInSeconds;
    private int _maxTimeInMinute;
    private int _currentTimeInMinute;
    private float _currentTimeInSeconds;
    private bool _timeIsOver;

    public bool TimeIsOver => _timeIsOver;

    private Coroutine _countdown;

    public event Action<string> TimeIsOvered;

    private void OnEnable()
    {
        _timeIsOver = false;
    }

    public void AddMinute(int desiredMinute)
    {
        _currentTimeInMinute += desiredMinute;
        _timeIsOver = false;
    }

    public void SetMaxMinute(int maxMinute)
    {
        _maxTimeInMinute = maxMinute;
    }

    public void SetMaxSecond(float maxSecond)
    {
        _maxTimeInSeconds = maxSecond;
    }

    public void LaunchCountdown()
    {
        StopCountdown();

        _countdown = StartCoroutine(Сountdown());
    }

    public void StopCountdown()
    {
        if (_countdown != null)
            StopCoroutine(_countdown);
    }

    private IEnumerator Сountdown()
    {
        _currentTimeInMinute = _maxTimeInMinute;
        _currentTimeInSeconds = _maxTimeInSeconds;
   //     _timerRender.UpdateTimer(_currentTimeInMinute, _currentTimeInSeconds);

        while (_currentTimeInMinute > 0 || _currentTimeInSeconds > 0)
        {
            _currentTimeInSeconds -= Time.deltaTime;

            if (_currentTimeInSeconds < 0)
            {
                _currentTimeInMinute--;
                _currentTimeInSeconds = MaxSecondInMinute;

                if (_currentTimeInMinute < 0)
                {
                    _currentTimeInMinute = 0;
                    _currentTimeInSeconds = 0;

                    break;
                }
            }

            _timerRender.UpdateTimer(_currentTimeInMinute, _currentTimeInSeconds);

            yield return null;
        }

            TimeIsOvered?.Invoke("ВРЕМЯ ВЫШЛО!");
/*
        if (_currentTimeInMinute == 0 && _currentTimeInSeconds == 0)
        {

            if (_timerRender != null)
            {
                _timerRender.UpdateTimer(_currentTimeInMinute, _currentTimeInSeconds);
            }
        }
*/
        _countdown = null;
    }
}
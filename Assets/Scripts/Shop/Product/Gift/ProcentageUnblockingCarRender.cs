using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProcentageUnblockingCarRender : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private float _duration = 1.25f;
    private Coroutine _fillCoroutine;

    public void UpdateSlider(float value)
    {
        FillTo(value);
    }

    private void FillTo(float targetValue)
    {
        if (_fillCoroutine != null)
            StopCoroutine(_fillCoroutine);

        _fillCoroutine = StartCoroutine(FillCoroutine(targetValue));
    }

    private IEnumerator FillCoroutine(float targetValue)
    {
        targetValue = Mathf.Clamp(targetValue, _slider.minValue, _slider.maxValue);

        float elapsed = 0f;
        float progress;
        _slider.value = 0f;
        
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            progress = Mathf.Clamp01(elapsed / _duration);
            _slider.value = Mathf.Lerp(0f, targetValue, progress);

            yield return null;
        }

        _slider.value = targetValue;
        _fillCoroutine = null;
    }
}
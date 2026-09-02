using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CarContainer : MonoBehaviour
{
    public Vector3 Position;
    public Quaternion Quaternion;
    public Vector3 Scale;

    [SerializeField] private Button _showCarInfoButton;
    [SerializeField] private CarProduct _carProduct;
    [SerializeField] private int _index;
    [SerializeField] private Car _carPrefab;
    [SerializeField] private CarInfo _carInfo;

    private float _maxAngle = 360;
    private bool _isOpen = false;
    private float _rotationSpeed = 45f;
    private Coroutine _smothlyZoom;
    private float _targetPositionZ = -50f;
    private float _duration = 0.5f;
    private RectTransform _rectTransform;

    public int Index => _index;
    public CarProduct CarGoods => _carProduct;
    public CarInfo CarInfo => _carInfo;
    public RectTransform RectTransform => _rectTransform;

    public event Action<CarContainer> CarGoodsInfoShowed;

    private void Awake()
    {
        _showCarInfoButton.onClick.AddListener(ShowCarInfo);
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (_carInfo is CarGifrInfo carGifrInfo)
        {
            if (_carProduct is CarGift carGift)
            {
                carGifrInfo.UpdateSlider(carGift.CompletedProcent);
            }
        }
        else if (_carInfo is CarGoodsInfo carGoodsInfo)
        {
            if (_carProduct is CarGoods carGoods)
            {
                carGoodsInfo.UpdateCarPrice(carGoods);
            }
        }

        _carInfo.UpdateInfo(_carProduct.Name, _carProduct.Speed.ToString());
    }

    private void ShowCarInfo()
    {
        CarGoodsInfoShowed?.Invoke(this);

        StartCoroutine(RotationCar());

        if (_smothlyZoom != null)
        {
            StopCoroutine(_smothlyZoom);
        }

        _smothlyZoom = StartCoroutine(SmothlyZoomCar(_targetPositionZ));
    }

    public void CloseCarInfo()
    {
        if (_smothlyZoom != null)
        {
            StopCoroutine(_smothlyZoom);
        }

        _smothlyZoom = StartCoroutine(SmothlyZoomCar(0));
        _isOpen = false;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    private IEnumerator SmothlyZoomCar(float targetPositionZ)
    {
        Vector3 targetPosition = new Vector3(_carPrefab.transform.localPosition.x, _carPrefab.transform.localPosition.y, targetPositionZ);
        float elapsedTime = 0f;

        Vector3 startPosition = _carPrefab.transform.localPosition;

        while (elapsedTime < _duration)
        {
            float time = Mathf.Clamp01(elapsedTime / _duration);
            _carPrefab.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _carPrefab.transform.localPosition = targetPosition;
    }

    private IEnumerator RotationCar()
    {
        _isOpen = true;
        Quaternion targetRotation = _carPrefab.transform.localRotation;

        while (_isOpen)
        {
            _carPrefab.transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);

            if (_carPrefab.transform.localRotation.y > _maxAngle)
            {
                _carPrefab.transform.localRotation = Quaternion.identity;

            }
            yield return null;
        }

        float elapsedTime = 0f;

        Quaternion startPosition = _carPrefab.transform.localRotation;

        while (elapsedTime < _duration)
        {
            float time = Mathf.Clamp01(elapsedTime / _duration);
            _carPrefab.transform.localRotation = Quaternion.Slerp(startPosition, targetRotation, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}

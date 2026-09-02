using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.CodeDom;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<CarContainer> _carContainers;
    [SerializeField] private WalletRender _walletRender;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _closeCarInfoButton;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentRectTransform;
    [SerializeField] private InsufficientFundsDisplay _insufficientFundsDisplay;
    [SerializeField] private CarProductSaver _carProductSaver;

    private CarProduct _carProduct;
    private CarContainer _currentCarContainer;
    private float _duration = 0.5f;
    private Wallet _wallet;

    private void Awake()
    {
        int index = 0;

        foreach (CarContainer carContainer in _carContainers)
        {
            carContainer.CarInfo.UpdateLockImage(_carProductSaver.IsCarBought(carContainer.CarGoods));
            
            carContainer.CarGoodsInfoShowed += ShowCarInfoButton;
            carContainer.SetIndex(index);
            index++;
        }
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(TrySellGoods);
        _closeCarInfoButton.onClick.AddListener(CloseCarInfoButton);
        _buyButton.gameObject.SetActive(false);
        _closeCarInfoButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(TrySellGoods);
        _closeCarInfoButton.onClick.RemoveListener(CloseCarInfoButton);
    }

    private void ShowCarInfoButton(CarContainer carContainer)
    {
        _carProduct = carContainer.CarGoods;
        _currentCarContainer = carContainer;
        _scrollRect.horizontal = false;
        StartCoroutine(SmoothlySetCarGoodsPosition(carContainer));
    }

    public void SetWallet(Wallet wallet)
    {
        _wallet = wallet;
        _wallet.SetWalletRender(_walletRender);
        _wallet.UpdateWalletInfo();
    }

    private void TrySellGoods()
    {
        if (_carProduct is CarGoods carGoods)
        {
            if (_carProduct.IsBought == false)
            {
                if (_wallet.TryRemoveCoin(carGoods.Price))
                {
                    carGoods.Buy();
                    _carProductSaver.SaveCarProduct(_carProduct);
                    _currentCarContainer.CarInfo.UpdateLockImage(true);
                }
                else
                {
                    _insufficientFundsDisplay.Open();
                }
            }
        }
    }

    private void CloseCarInfoButton()
    {
        _scrollRect.horizontal = true;
        _buyButton.gameObject.SetActive(false);
        _closeCarInfoButton.gameObject.SetActive(false);
        _currentCarContainer.CloseCarInfo();
    }

    private IEnumerator SmoothlySetCarGoodsPosition(CarContainer carContainer)
    {
        float elapsedTime = 0f;
        Vector2 targetPosition = new Vector2(carContainer.Index * -_currentCarContainer.RectTransform.rect.width, 0);
        Vector2 startPosition = _contentRectTransform.anchoredPosition;

        _closeCarInfoButton.gameObject.SetActive(true);

        while (elapsedTime < _duration)
        {
            float time = Mathf.Clamp01(elapsedTime / _duration);
            _contentRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _contentRectTransform.anchoredPosition = targetPosition;

        _buyButton.gameObject.SetActive(true);
    }
}
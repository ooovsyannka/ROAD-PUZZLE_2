using System;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private Button _selectCarButton;
    [SerializeField] private CarContainersHolder _carContainersHolder;
    [SerializeField] private CarProductSaver _carProductSaver;

    private void OnEnable()
    {
        _selectCarButton.onClick.AddListener(SwitchSelectCar);
    }

    private void OnDisable()
    {
        _selectCarButton.onClick.RemoveListener(SwitchSelectCar);
    }

    private void SwitchSelectCar()
    {
        CarContainer carContainer = _carContainersHolder.CurrentCarContainer;
        CarProduct carProduct = carContainer.CarProduct;

        if (_carProductSaver.IsCarSelected(carProduct))
        {
            if (_carProductSaver.IsLastSelectCarProduct())
            {
                _carProductSaver.DeletSelectCarProduct(carProduct);
                carContainer.CarInfo.UpdateSelectImage(false);
            }
            else
            {
                print("Последнюю машинку нельзя убрать  ");
            }
        }
        else
        {
            _carProductSaver.SaveSelectCarProduct(carProduct);
            carContainer.CarInfo.UpdateSelectImage(true);
        }
    }
}
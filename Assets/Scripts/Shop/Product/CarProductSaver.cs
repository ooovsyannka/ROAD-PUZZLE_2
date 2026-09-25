using System;
using System.Collections.Generic;
using UnityEngine;

public class CarProductSaver : MonoBehaviour
{
    public List<CarContainer> carContainers;
    public CarProduct DefaultCarProduct;


    private const string BuyCar = nameof(BuyCar);
    private const string SelectCarCount = nameof(SelectCarCount);
    private const string SelectCar = nameof(SelectCar);
    private const int NumberPurchasedCar = 1;

    private int _selectCarCount;

    private void Awake()
    {
        _selectCarCount = PlayerPrefs.GetInt($"{SelectCarCount}");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (CarContainer carContainer in carContainers)
            {
                PlayerPrefs.DeleteKey($"{BuyCar} {carContainer.CarProduct.Index}");
                PlayerPrefs.DeleteKey($"{SelectCar} {carContainer.CarProduct.Index}");
            }

            _selectCarCount = 1;

            PlayerPrefs.SetInt($"{SelectCarCount}", _selectCarCount);
            PlayerPrefs.SetInt($"{BuyCar} {DefaultCarProduct.Index}", NumberPurchasedCar);
            PlayerPrefs.SetInt($"{SelectCar} {DefaultCarProduct.Index}", NumberPurchasedCar);
            print("Машини почистины");
        }

        print(_selectCarCount);
    }

    public void SaveCarProduct(CarProduct carProduct)
    {
        PlayerPrefs.SetInt($"{BuyCar} {carProduct.Index}", NumberPurchasedCar);
    }

    public void SaveSelectCarProduct(CarProduct carProduct)
    {
        _selectCarCount++;
        PlayerPrefs.SetInt($"{SelectCarCount}", _selectCarCount);
        PlayerPrefs.SetInt($"{SelectCar} {carProduct.Index}", NumberPurchasedCar);
    }

    public void DeletSelectCarProduct(CarProduct carProduct)
    {
        _selectCarCount--;
            PlayerPrefs.DeleteKey($"{SelectCar} {carProduct.Index}");
            PlayerPrefs.SetInt($"{SelectCarCount}", _selectCarCount);
    }

    public bool IsLastSelectCarProduct()
    {
        return _selectCarCount > 1;
    }

    public bool IsCarBought(CarProduct carProduct)
    {
        return PlayerPrefs.GetInt($"{BuyCar} {carProduct.Index}", 0) == NumberPurchasedCar;
    }

    public bool IsCarSelected(CarProduct carProduct)
    {
        return PlayerPrefs.GetInt($"{SelectCar} {carProduct.Index}", 0) == NumberPurchasedCar;
    }
}
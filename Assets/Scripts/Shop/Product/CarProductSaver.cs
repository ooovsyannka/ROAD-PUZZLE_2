using System.Collections.Generic;
using UnityEngine;

public class CarProductSaver : MonoBehaviour
{
    public List<CarContainer> carContainers;
    public CarProduct DefaultCarProduct;


    private const string BuyCar = nameof(BuyCar);
    private const string SelectCar = nameof(SelectCar);
    private const int NumberPurchasedCar = 1;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (CarContainer carContainer in carContainers)
            {
                PlayerPrefs.DeleteKey($"{BuyCar} {carContainer.CarProduct.Index}");
            PlayerPrefs.DeleteKey($"{SelectCar} {carContainer.CarProduct.Index}");
            }

            PlayerPrefs.SetInt($"{BuyCar} {DefaultCarProduct.Index}", NumberPurchasedCar);
            PlayerPrefs.SetInt($"{SelectCar} {DefaultCarProduct.Index}", NumberPurchasedCar);
            print("Машини почистины");
        }
    }

    public void SaveCarProduct(CarProduct carProduct)
    {
        PlayerPrefs.SetInt($"{BuyCar} {carProduct.Index}", NumberPurchasedCar);
    }

    public void SaveSelectCarProduct(CarProduct carProduct)
    {
        PlayerPrefs.SetInt($"{SelectCar} {carProduct.Index}", NumberPurchasedCar);
    }

    public void DeletSelectCarProduct(CarProduct carProduct)
    {
        PlayerPrefs.DeleteKey($"{SelectCar} {carProduct.Index}");
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
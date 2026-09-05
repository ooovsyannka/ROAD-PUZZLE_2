using System.Collections.Generic;
using UnityEngine;

public class CarProductSaver : MonoBehaviour
{
    public List<CarContainer> carContainers;
    public CarProduct CarProduct;


    private const string BuyCar = nameof(BuyCar);
    private const int NumberPurchasedCar = 1;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            foreach (CarContainer carContainer in carContainers)
            {
                PlayerPrefs.DeleteKey($"{BuyCar} {carContainer.CarGoods.Index}");
            }

            PlayerPrefs.SetInt($"{BuyCar} {CarProduct.Index}", NumberPurchasedCar);
            print("Машини почистины");
        }
    }

    public void SaveCarProduct(CarProduct carProduct)
    {
        PlayerPrefs.SetInt($"{BuyCar} {carProduct.Index}", NumberPurchasedCar);
    }

    public bool IsCarBought(CarProduct carProduct)
    {
        return PlayerPrefs.GetInt($"{BuyCar} {carProduct.Index}", 0) == NumberPurchasedCar;
    }
}
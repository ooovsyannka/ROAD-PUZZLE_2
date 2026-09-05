using UnityEngine;
using TMPro;

public class CarGoodsInfo : CarInfo
{
    [SerializeField] private TextMeshProUGUI _price;

    public void UpdateCarPrice(CarGoods carGoods)
    {
        _price.text = carGoods.Price.ToString();
    }
}
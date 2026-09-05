using UnityEngine;
[CreateAssetMenu(fileName = "NewGoodsCar", menuName = "Goods/CarGoods", order = 1)]
public class CarGoods : CarProduct
{
    [SerializeField] private int _price;

    public int Price => _price;

    public void Buy()
    {
        _isBought = true;
    }
}
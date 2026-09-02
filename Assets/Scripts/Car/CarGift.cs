using UnityEngine;

[CreateAssetMenu(fileName = "NewGiftCar", menuName = "Goods/CarGift", order = 1)]

public class CarGift : CarProduct
{
    [SerializeField] private float _completedProcent;

    public float CompletedProcent => _completedProcent;

    public void AddProcent(float procent)
    {
        _completedProcent += procent;
    }
}

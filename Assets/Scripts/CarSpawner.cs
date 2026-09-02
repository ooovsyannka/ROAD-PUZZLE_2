using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour  
{
    [SerializeField] private List<CarProduct> _carProducts;
    [SerializeField] private CarProductSaver _carProductSaver;
    private List<Car> _carPrefabs = new List<Car>();    

    private Spawner<Car> _spawner;

    private void Awake()
    {
        foreach(CarProduct carProduct in _carProducts)
        {
            if (_carProductSaver.IsCarBought(carProduct))
            {
                _carPrefabs.Add(carProduct.CarPrefab);
            }
        }

        if (_spawner == null)
            _spawner = new Spawner<Car>(_carPrefabs);
    }

    public Car GetRandomCar(Vector3 carPosition)
    {
        foreach (CarProduct carProduct in _carProducts)
        {
            if (_carProductSaver.IsCarBought(carProduct))
            {
                _carPrefabs.Add(carProduct.CarPrefab);
            }
        }

        if (_spawner == null)
        {
            _spawner = new Spawner<Car>(_carPrefabs);
        }

        Car car = _spawner.SpawnObjectFromList(carPosition);
        car.Died += ReturnCarInPool;

        return car;
    }

    private void ReturnCarInPool(Car car)
    {
        _spawner.ReturnObjectInPool(car);
        car.Died -= ReturnCarInPool;
    }
}
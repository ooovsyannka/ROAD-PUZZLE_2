using System;
using System.Collections.Generic;
using UnityEngine;

public class CarContainersHolder : MonoBehaviour
{
    [SerializeField] private List<CarContainer> _carContainers;
    [SerializeField] private CarProductSaver _carProductSaver;
    
    private CarContainer _currentCarContainer;
    
    public event Action<CarContainer> CarContainerSelected;
    
    public CarContainer CurrentCarContainer => _currentCarContainer;
    
    private void Awake()
    {
        int index = 0;

        foreach (CarContainer carContainer in _carContainers)
        {
            carContainer.CarInfo.UpdateLockImage(_carProductSaver.IsCarBought(carContainer.CarProduct));
            carContainer.CarInfo.UpdateSelectImage(_carProductSaver.IsCarSelected(carContainer.CarProduct));

            carContainer.CarGoodsInfoShowed += SetCurrentCarContainer;
            carContainer.SetIndex(index);
            index++;
        }
    }

    private void SetCurrentCarContainer(CarContainer carContainer)
    {
        _currentCarContainer = carContainer;
        
        CarContainerSelected?.Invoke(_currentCarContainer);
    }
}
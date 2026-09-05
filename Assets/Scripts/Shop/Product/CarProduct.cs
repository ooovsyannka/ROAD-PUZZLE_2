using UnityEngine;

public abstract class CarProduct : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Car _carPrefab;
    [SerializeField] private int _index;
    [SerializeField] protected bool _isBought;

    public string Name => _name;
    public int Speed => _carPrefab.Speed;
    public bool IsBought => _isBought;
    public  int Index => _index;
    public Car CarPrefab => _carPrefab;
}

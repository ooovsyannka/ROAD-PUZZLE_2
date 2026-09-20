using UnityEngine;

public abstract class CarProduct : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Car _carPrefab;
    [SerializeField] private int _index;

    protected bool _isBought;
    private bool _isSelected;

    public string Name => _name;
    public int Speed => _carPrefab.Speed;
    public bool IsBought => _isBought;
    public bool IsSelected => _isSelected;
    public int Index => _index;
    public Car CarPrefab => _carPrefab;
    
    public void Select()
    {
        _isSelected = true;
    }

    public void Deselect()
    {
        _isSelected = false;
    }
}
using UnityEngine;

[System.Serializable]
public class GridSection
{
    [SerializeField] private int _maxLengthY;
    [SerializeField] private int _maxLengthX;

    public int MaxLengthY => _maxLengthY;
    public int MaxLengthX => _maxLengthX;
}

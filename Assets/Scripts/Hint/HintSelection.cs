using UnityEngine;
[System.Serializable]

public class HintSelection 
{
    [SerializeField] private Sprite _hintImage;
    [SerializeField] private int _cost;

    public Sprite HintImage => _hintImage;
    public int Cost => _cost;
}
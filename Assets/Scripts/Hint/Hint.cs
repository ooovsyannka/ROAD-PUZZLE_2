using UnityEngine;
using UnityEngine.UI;

public class Hint : Window
{
    [SerializeField] private Image _hintImage;
    [SerializeField] private HintRender _hintRender;
    [SerializeField] private Button _closeButton;

    private bool _isBought;
    private int _cost;

    public bool IsBought => _isBought;
    public int Cost => _cost;


    private void OnEnable()
    {
        Close();
        _isBought = false;
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);        
    }

    public void SetCost(int cost)
    {
        _cost = cost;
        _hintRender.UpdatePrice(cost); 
    }

    public void SetSprite(Sprite sprite)
    {
        _hintImage.sprite = sprite;
    }

    public void Buy()
    {
        _isBought = true;
    }
}

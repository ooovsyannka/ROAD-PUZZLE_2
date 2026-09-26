using UnityEngine;
using UnityEngine.UI;

public class AddCoinButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private CoinShop _coinShop;

    private void OnEnable()
    {
        if (_button != null)
        {
            print("correct");
            _button.onClick.AddListener(OnButtonAction);
        }

        else
        {
            
            print("uncorrect");
        }

    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonAction);
    }

    protected void OnButtonAction()
    {
        print(1);
        _coinShop.Open();
    }
}

using TMPro;
using UnityEngine;

public class HintRender : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _price;

    public void UpdatePrice(int price)
    {
        _price.text = price.ToString(); 
    }
}
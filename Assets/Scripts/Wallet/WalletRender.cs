using TMPro;
using UnityEngine;

public class WalletRender : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _walletInfo;

    public void ShowWalletInfo(int currentCoinCount)
    {
        _walletInfo.text = currentCoinCount.ToString();
    }
}
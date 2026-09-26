using UnityEngine;

[RequireComponent(typeof(WalletSaver))]

public class Wallet : MonoBehaviour
{
    [SerializeField] private WalletRender _walletRender;
    [SerializeField] private WalletSaver _walletSaver;

    private int _countCoin;

    private void OnEnable()
    {
        LoadCoinsFromSave();
        ShowWalletCount();
    }
    
    public void AddCoin(int count)
    {
        LoadCoinsFromSave();
        _countCoin += count;
        SaveCoinsInSaver();
        ShowWalletCount();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddCoin(1000);
        }
    }

    public bool TryRemoveCoin(int minusCount)
    {
        if (_countCoin - minusCount < 0)
        {
            return false;
        }

        _countCoin -= minusCount;

        SaveCoinsInSaver();
        ShowWalletCount();

        return true;
    }

    private void ShowWalletCount()
    {
        _walletRender.ShowWalletInfo(_countCoin);
    }


    private void LoadCoinsFromSave()
    {
        _countCoin = _walletSaver.LoadCoins();
    }
    
    private void SaveCoinsInSaver()
    {
        _walletSaver.TrySaveCoinInWalet(_countCoin);
    }
}

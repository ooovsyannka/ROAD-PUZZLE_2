using UnityEngine;

[RequireComponent(typeof(WalletSaver))]

public class Wallet : MonoBehaviour
{
    [SerializeField] private WalletRender _walletRender;
    [SerializeField] private WalletSaver _walletSaver;

    private int _countCoin;

    public int CountCoin => _countCoin;

    private void OnEnable()
    {
        LoadCoinsFromSave();
    }

    public void AddCoin(int count)
    {
        LoadCoinsFromSave();
        _countCoin += count;
        SaveCoinsInSave();
        UpdateWalletInfo();
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

        SaveCoinsInSave();
        UpdateWalletInfo();

        return true;
    }

    public void UpdateWalletInfo()
    {
        _walletRender.ShowWalletInfo(_countCoin);
    }

    public void SetWalletRender(WalletRender walletRender)
    {
        _walletRender = walletRender;
    }

    private void LoadCoinsFromSave()
    {
        _countCoin = _walletSaver.LoadCoins();
    }
    private void SaveCoinsInSave()
    {
        _walletSaver.SaveCoinInWalet(_countCoin);
    }
}

using UnityEngine;

public class WalletSaver : MonoBehaviour
{
    private const string CoinSave = nameof(CoinSave);

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.DeleteKey(CoinSave);
            print("Монеты удалены ");
        }
    }

    public void SaveCoinInWalet(int coinInWallet)
    {
        if (IsCorrectCount(coinInWallet))
        {
            PlayerPrefs.SetInt(CoinSave, coinInWallet);
        }
    }

    public void AddCoinInSave(int desiredCount)
    {
        if (IsCorrectCount(desiredCount))
        {
            SaveCoinInWalet(LoadCoins() + desiredCount);
        }
    }

    public int LoadCoins() =>
        PlayerPrefs.GetInt(CoinSave);

    private bool IsCorrectCount(int count)=>
        count > 0;
}
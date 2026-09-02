using UnityEngine;
using IJunior.TypedScenes;

public class MainMenuSceneLoader : MonoBehaviour, ISceneLoadHandler<int>
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private WalletSaver _walletSaver;

    public void OnSceneLoaded(int count)
    {
        _wallet.AddCoin(count);
    }
}
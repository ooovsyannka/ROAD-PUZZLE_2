using UnityEngine;
using IJunior.TypedScenes;

public class ShopSceneLoader : MonoBehaviour, ISceneLoadHandler<Wallet>
{
    [SerializeField] private Shop _shop;

    public void OnSceneLoaded(Wallet wallet)
    {
        _shop.SetWallet(wallet);
    }
}

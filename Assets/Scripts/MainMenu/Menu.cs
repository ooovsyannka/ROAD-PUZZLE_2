using UnityEngine;
using IJunior.TypedScenes;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] private LevelHolder _levelHolder;
    [SerializeField] private Button _levelButton;
    [SerializeField] private Button _infinityButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private MenuRender _menuRender;

    private bool _canLoadScene;
    private LevelData _currentLevel;

    private void Start()
    {
        //LaunchLevel();
    }

    private void OnEnable()
    {
        _canLoadScene = true;
        _levelButton.onClick.AddListener(LaunchLevel);
        _infinityButton.onClick.AddListener(LaunchInfinityLevel);
        _shopButton.onClick.AddListener(OpenShop);
        _wallet.UpdateWalletInfo();

        if (_levelHolder.TryGetLevel(out LevelData levelData))
        {
            _currentLevel = levelData;
            _menuRender.ShowLevelNumber(_currentLevel.Index );
        }
        else
        {
            _menuRender.ShowMessage();
        }
    }

    private void OnDisable()
    {
        _levelButton.onClick.RemoveListener(LaunchLevel);
        _infinityButton.onClick.RemoveListener(LaunchInfinityLevel);
        _shopButton.onClick.RemoveListener(OpenShop);
    }

    private void LaunchLevel()
    {
        if (_currentLevel == null)
        {
            print("All levels complete");
            return;
        }

        TryLoadScene(() => LevelScene.Load(_currentLevel));
    }

    private void LaunchInfinityLevel()
    {
        TryLoadScene(() => Infinity.Load());
    }

    private void OpenShop()
    {
        TryLoadScene(() => ShopScene.Load(_wallet));
    }

    private void TryLoadScene(Action loadScene)
    {
        if (_canLoadScene == false)
            return;

        loadScene?.Invoke();
        _canLoadScene = false;
    }
}

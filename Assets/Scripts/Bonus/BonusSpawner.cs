using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Clock _clockPrefab;

    private float _procentSpawnCoin = 0.65f;
    private float _procentSpawnClock = 0.75f;
    private float _maxProcentage = 1f;

    private Spawner<Coin> _coinSpawner;
    private Spawner<Clock> _clockSpawner;

    private void OnEnable()
    {
        _coinSpawner = new Spawner<Coin>(_coinPrefab);
        _clockSpawner = new Spawner<Clock>(_clockPrefab);
    }

    public bool TrySpawnCoin(Vector3 positionOnGird, out Coin coin)
    {
        coin = null;

        if (GetRandomProcentage() > _procentSpawnCoin)
        {
            coin = _coinSpawner.Spawn(positionOnGird, null);
            coin.Died += ReturnCoinInPool;

            return true;
        }

        return false;
    }

    public bool TrySpawnClock(Vector3 positionOnGird, out Clock clock)
    {
        clock = null;

        if (GetRandomProcentage() > _procentSpawnClock)
        {
            clock = _clockSpawner.Spawn(positionOnGird, null);
            clock.Died += ReturnCoinInPool;

            return true;
        }

        return false;
    }

    private void ReturnCoinInPool(Bonus bonus)
    {
        if (bonus is Coin coin)
        {
            _coinSpawner.ReturnObjectInPool(coin);
            coin.Died -= ReturnCoinInPool;
        }
        else if (bonus is Clock clock)
        {
            _clockSpawner.ReturnObjectInPool(clock);
            clock.Died -= ReturnCoinInPool;
        }
    }

    private float GetRandomProcentage()
    {
        return Random.Range(0, _maxProcentage);
    }
}
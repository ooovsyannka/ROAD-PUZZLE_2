using UnityEngine;


public class BonusCollectionHandler : MonoBehaviour
{
    [SerializeField] private BonusSpawner _bonusSpawner;

    private Grid _grid;
    private Timer _timer;
    private int _collectedСoins = 0;

    public int CollectedCoins => _collectedСoins;

    public void Initialize( Grid grid, Timer timer )
    {
        _grid = grid;
        _timer = timer;
    }
    
    public void AttemptSpawnBonus()
    {
        Vector3 cellPosition = _grid.TryGetRandomEmptyCell().transform.position;

        if (cellPosition != Vector3.zero)
        {
            if (_bonusSpawner.TrySpawnClock(cellPosition, out Clock clock))
            {
                clock.OnCollected += CollectClock;
            }
            else if (_bonusSpawner.TrySpawnCoin(cellPosition, out Coin coin))
            {
                coin.OnCollected += CollectCoin;
            }
        }
    }

    private void CollectCoin(Coin coin, int countCoin)
    {
        coin.OnCollected -= CollectCoin;
        _collectedСoins += countCoin;
    }

    private void CollectClock(Clock clock, int timeCount)
    {
        _timer.AddMinute(timeCount);
        clock.OnCollected -= CollectClock;
    }
}
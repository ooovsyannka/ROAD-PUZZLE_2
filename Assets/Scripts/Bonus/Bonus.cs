using JetBrains.Annotations;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [SerializeField] private int _bonusCount;
    [SerializeField] private BonusAnimation _bonusAnimation;
    [SerializeField] private ParticleSystem _particleSystem;

    public ParticleSystem ParticleSystem => _particleSystem;
    public int BonusCount => _bonusCount;
    protected BonusAnimation BonusAnimation => _bonusAnimation;

    protected abstract void Die();
}
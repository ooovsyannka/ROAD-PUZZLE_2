using System;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : Bonus
{
    public event Action<Coin> Died;
    public event Action<Coin, int> OnCollected;

    private void OnEnable()
    {
        BonusAnimation.OnAnimationFinished += Die;
    }

    private void OnDisable()
    {
        BonusAnimation.OnAnimationFinished -= Die;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car _))
        {
            BonusAnimation.PlayDieAnimation();
            OnCollected?.Invoke(this, BonusCount);
            ParticleSystem.Play();
        }
    }

    protected override void Die()
    {
        Died?.Invoke(this);
    }
}

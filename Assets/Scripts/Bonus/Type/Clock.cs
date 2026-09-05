using System;
using UnityEngine;

public class Clock : Bonus
{
    public event Action<Clock> Died;
    public event Action<Clock,int> OnCollected;

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
        }
    }

    protected override void Die()
    {
        Died?.Invoke(this);
    }
}

using System;
using UnityEngine;

public class BonusAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action OnAnimationFinished;

    public void PlayDieAnimation()
    {
        _animator.SetTrigger(AnimationData.Params.Die);
    }

    private void Finish()
    {
        OnAnimationFinished?.Invoke();
    }
}

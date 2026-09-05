using System;
using UnityEngine;

public class StreetGroundAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action AnimationFinished;

    public void PlayGrowAnimation()
    {
        _animator.SetTrigger(AnimationData.Params.Grow);
    }

    public void PlaySmallerAnimation()
    {
        _animator.SetTrigger(AnimationData.Params.Smaller);
    }

    private void AnimationFinish()
    {
        AnimationFinished?.Invoke();
    }
}
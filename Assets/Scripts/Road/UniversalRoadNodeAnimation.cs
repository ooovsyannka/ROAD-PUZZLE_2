using System;
using UnityEngine;

public class UniversalRoadNodeAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action Finished;

    public void PlayAnimation()
    {
        _animator.SetBool(AnimationData.Params.IsBroken, true);
    }
    public void StopAnimation()
    {
        _animator.SetBool(AnimationData.Params.IsBroken, false);
    }

    private void OnFinish()
    {
        transform.rotation = Quaternion.identity;
        Finished?.Invoke();
    }
}
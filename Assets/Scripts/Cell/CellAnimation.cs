using System;
using UnityEngine;

public class CellAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public event Action Finished;

    public void PlayAnimation()
    {
        _animator.SetTrigger(AnimationData.Params.Rotate);
    }

    private void OnFinish()
    {
        transform.rotation = Quaternion.identity;
        Finished?.Invoke();
    }
}
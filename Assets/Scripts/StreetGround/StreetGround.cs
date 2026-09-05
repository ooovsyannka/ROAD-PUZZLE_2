using System;
using UnityEngine;

public class StreetGround : MonoBehaviour
{
    [SerializeField] private StreetLamp _streetLamp;
    [SerializeField] private StreetGroundAnimation _streetGroundAnimation;

    public event Action<StreetGround>  Died;

    public void PlayGrowAnimation()
    {
        _streetGroundAnimation.PlayGrowAnimation();
    }

    public void PlaySmallerAnimation()
    {
        _streetGroundAnimation.PlaySmallerAnimation();
        _streetGroundAnimation.AnimationFinished += Die;
    }

    public void RotatoinStretLampByCell(Cell cell)
    {
        _streetLamp.LookAtTarget(cell.transform);
    }

    public void SetRotationStreetLamp(Quaternion quaternion)
    {
        _streetLamp.SetRotation(quaternion);
    }
    

    public void Die()
    {
        _streetGroundAnimation.AnimationFinished -= Die;
        Died?.Invoke(this);
    }
}

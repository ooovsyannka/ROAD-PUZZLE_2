using UnityEngine;

public static class AnimationData
{
    public static class Params
    {
        public static readonly int Die = Animator.StringToHash(nameof(Die));
        public static readonly int Grow = Animator.StringToHash(nameof(Grow));
        public static readonly int Smaller = Animator.StringToHash(nameof(Smaller));
        public static readonly int Break = Animator.StringToHash(nameof(Break));
        public static readonly int Rotate = Animator.StringToHash(nameof(Rotate));
        public static readonly int IsDie = Animator.StringToHash(nameof(IsDie));
        public static readonly int IsBroken = Animator.StringToHash(nameof(IsBroken));
    }
}
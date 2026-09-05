using UnityEngine;

public static class MathExtensions
{
    private static int s_multiple = 15;

    public static int Multiple => s_multiple;

    public static int RoundIntToNearest(this float value)
    {
        return Mathf.RoundToInt(value / s_multiple) * s_multiple;
    }

    public static Vector3 RoundVector3ToNearest(this Vector3 value)
    {
        return new Vector3(
            value.x.RoundIntToNearest(),
            value.y.RoundIntToNearest(),
            value.z.RoundIntToNearest()
        );
    }

    public static Vector3 ConvertVectorToIndex(this Vector3 value)
    {
        return new Vector3(value.x / s_multiple, 0, value.z / s_multiple);
    }

    public static int ConvertIntToIndex(this int value)
    {
        return value / s_multiple;
    }
}

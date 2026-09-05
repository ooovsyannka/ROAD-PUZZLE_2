using System.Collections.Generic;
using UnityEngine;

public class RoadPreview : MonoBehaviour
{
    public void SetParent(Transform parent)
    {
        transform.parent = parent;

        if (parent != null)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    public void ShowPreviwPosition(Vector3 position)
    {
        transform.position = position;
    }
}

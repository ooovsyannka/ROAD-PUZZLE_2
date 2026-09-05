using UnityEngine;

public class MergePointHolder : MonoBehaviour
{
    [SerializeField] private MergePoint _enterPoint;
    [SerializeField] private MergePoint _exitPoint;

    public MergePoint EnterPoint => _enterPoint;
    public MergePoint ExitPoint => _exitPoint;

    public void SetMergePoint(MergePoint enterPoint,    MergePoint exitPoint)
    {
        _enterPoint = enterPoint;
        _exitPoint = exitPoint;
    }
}
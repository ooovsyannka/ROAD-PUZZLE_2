using UnityEngine;

public class RoadRender : MonoBehaviour
{
    [SerializeField] private RoadMarkingRender _roadMarkingRender;
    [SerializeField] private SidewalkRender _sidewalkRender;

    public void SetDisconnectRender()
    {
        _roadMarkingRender.SetOriginalMaterial();
        _sidewalkRender.SetDarkColor();
    }

    public void SetConnectRender()
    {
        _roadMarkingRender.SetEmisionMaterila();
        _sidewalkRender.SetOriginalColor();
    }

    public void SetDarkRenderSideWalk()
    {
        _sidewalkRender.SetBlackColor();
    }
}

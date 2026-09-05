using UnityEngine;
using IJunior.TypedScenes;

public class LevelSceneLoader : MonoBehaviour, ISceneLoadHandler<LevelData>
{
    [SerializeField] private Level _level;
    [SerializeField] private RoadBuilder _roadBuilder;
    [SerializeField] private RoadDrager _roadDrager;
    [SerializeField] private GridInstantiator _gridInstantiator;
    [SerializeField] private RoadInstantiator _roadInstantiator;
    [SerializeField] private StreetGroundInstantiator _streetGroundInstantiator;
    [SerializeField] private Camera _camera;
    [SerializeField] private LightCycle _lightCycle;
    [SerializeField] private Light _light;

    public void OnSceneLoaded(LevelData levelData)
    {
        InitializeLevel(levelData);
    }

    private void InitializeLevel(LevelData levelData)
    {
        Grid grid = _gridInstantiator.InstantiateGrid(levelData.GridSection);
        _roadDrager.SetGrid(grid);
        _roadInstantiator.InstantiateRoads(levelData);

        _camera.transform.position = levelData.CameraTransformSelection.Position;
        _camera.transform.rotation = levelData.CameraTransformSelection.Rotation;

        _roadBuilder.SetGrid(grid);
        _roadBuilder.SetFinishRoads(_roadInstantiator.FinishRoads);
        _roadBuilder.SetStartRoads(_roadInstantiator.StartRoads);

        _streetGroundInstantiator.TryInstantiateStreetGround(levelData.StreetGroundTransformSelections, grid);
        _light.intensity = _lightCycle.GetLightIntensity(levelData.TimeOfDay);

        _level.SetLevelInfo(grid, _roadInstantiator.StartRoads, _roadInstantiator.FinishRoads, _roadInstantiator.Roads, levelData);
    }
}

using IJunior.TypedScenes;
using UnityEngine;

public class InfinityLevelSceneLoader : MonoBehaviour, ISceneLoadHandler<Grid>
{
    [SerializeField] private InfinityLevel _infinityLevel;
    [SerializeField] private Grid _grid;
    
    public void OnSceneLoaded(Grid grid)
    {
        _infinityLevel.SetGrid(grid);
    }
}
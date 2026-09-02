using IJunior.TypedScenes;

public class RestartButton : ActionButton
{
    private LevelMode _levelMode;
    private LevelData _levelData;

    public void SetLevelMode(LevelMode levelMode, LevelData levelData = null)
    {
        _levelMode = levelMode;

        if (levelData != null)
        {
            _levelData = levelData;
        }
    }

    protected override void OnButtonAction()
    {
        if (_levelMode == LevelMode.Infinity)
        {
            Infinity.Load();
        }
        else if (_levelMode == LevelMode.Classic)
        {
            LevelScene.Load(_levelData);
        }
    }
}
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
        switch (_levelMode)
        {
            case LevelMode.Infinity:
                Infinity.Load();
                break;
            case LevelMode.Classic:
                LevelScene.Load(_levelData);
                break;
        }
    }
}
using IJunior.TypedScenes;

public class HomeButton : ActionButton
{
    protected override void OnButtonAction()
    {
        MainMenu.Load();
    }
}

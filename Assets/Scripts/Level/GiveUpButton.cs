using System;

public class GiveUpButton : ActionButton
{
    public  event Action<string> OnGiveUp;

    protected override void OnButtonAction()
    {
        OnGiveUp?.Invoke("ВЫ СДАЛИСЬ");
    }
}

using UnityEngine;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;

    protected int WindowAlpha = 1;
    protected CanvasGroup WindowGroup => _windowGroup;

    public virtual void Open()
    {
        WindowGroup.alpha = WindowAlpha;
        WindowGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.blocksRaycasts = false;
    }
}
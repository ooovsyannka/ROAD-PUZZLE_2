using System;
using UnityEngine;

public class CloseWindowButton : ActionButton
{
    [SerializeField] private Window _window;

    public event Action WindowClose;

    protected override void OnButtonAction()
    {
        _window.Close();
        WindowClose?.Invoke();
    }
}
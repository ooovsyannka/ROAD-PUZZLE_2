using System.Collections;
using UnityEngine;

public class InsufficientFundsDisplay : Window
{
    private float _displayDuration = 2f;
    private Coroutine _showAndAutoHide;
    private WaitForSeconds _displayWait;

    private void OnEnable()
    {
        Close();
    }

    private void Awake()
    {
        _displayWait = new WaitForSeconds(_displayDuration);
    }

    public override void Open()
    {
        if(_showAndAutoHide != null)   
            StopCoroutine(_showAndAutoHide);

        _showAndAutoHide = StartCoroutine(ShowAndAutoHide());
    }

    private IEnumerator ShowAndAutoHide()
    {
        base.Open();

        yield return _displayWait;

        Close();
    }
}
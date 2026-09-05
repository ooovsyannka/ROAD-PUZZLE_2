using System.Collections;
using TMPro;
using UnityEngine;

public class ComboWindow : Window
{
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private float _hideDelay = 2f;

    private WaitForSeconds _hideDelayWait;
    private Coroutine _hideCoroutine;

    private void Awake()
    {
        _hideDelayWait = new WaitForSeconds(_hideDelay);
    }

    private void OnEnable()
    {
        Close();
    }

    public override void Open()
    {
        base.Open();

        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        _hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    public void SetComboCount(int comboCount)
    {
        _comboText.text = comboCount.ToString();
    }

    private IEnumerator HideAfterDelay()
    {
        yield return _hideDelayWait;

        Close();
    }
}

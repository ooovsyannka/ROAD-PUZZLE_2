using UnityEngine;
using UnityEngine.UI;

public abstract class ActionButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    public Button Button =>  _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonAction);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonAction);
    }

    protected abstract void OnButtonAction();
}

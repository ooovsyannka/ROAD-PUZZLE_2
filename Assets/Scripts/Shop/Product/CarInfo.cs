using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public abstract class CarInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _lock;
    [SerializeField] private Sprite _closeLock;
    [SerializeField] private Sprite _openLock;
    [SerializeField] private TextMeshProUGUI _speed;

    public void UpdateLockImage(bool IsBougth)
    {
        if (IsBougth == true)
        {
            _lock.sprite = _openLock;
        }
        else
        {
            _lock.sprite = _closeLock;
        }
    }

    public void UpdateInfo(string name, string speed)
    {
        _name.text = name;
        _speed.text = speed;
    }
}

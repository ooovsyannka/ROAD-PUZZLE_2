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
    [SerializeField] private Image _selectImage;
    [SerializeField] private Sprite _selectSprite;

    public void UpdateLockImage(bool IsBougth)
    {
        _lock.sprite = IsBougth ? _openLock : _closeLock;
    }

    public void UpdateSelectImage(bool isSelect)
    {
        _selectImage.sprite = isSelect ? _selectSprite : null;
    }

    public void UpdateInfo(string name, string speed)
    {
        _name.text = name;
        _speed.text = speed;
    }
}

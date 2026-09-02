using System.Collections;
using UnityEngine;

public class SidewalkRender : MonoBehaviour
{
    [SerializeField] private Material _orignalMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Coroutine _smothlyChangeMaterial;
    private Material _clonMaterial;
    private Color _originColor;
    private Color _darkColor;
    private Color _blackColor;

    private void Awake()
    {
        _clonMaterial = new Material(_orignalMaterial);
        _originColor = _clonMaterial.color;
        _meshRenderer.material = _clonMaterial;
    }

    private void OnEnable()
    {
        _darkColor = _originColor * Color.gray;
        _clonMaterial.color = _darkColor;
        _blackColor = Color.black;
    }

    public void SetOriginalColor()
    {
        LaunchSmothlyChangeColor(_originColor);
    }

    public void SetDarkColor()
    {
        LaunchSmothlyChangeColor(_darkColor);
    }

    public void SetBlackColor()
    {
        LaunchSmothlyChangeColor(_blackColor);
    }

    private void LaunchSmothlyChangeColor(Color color)
    {
        if (_smothlyChangeMaterial != null)
            StopCoroutine(_smothlyChangeMaterial);

        _smothlyChangeMaterial = StartCoroutine(SmothlyChangeColor(color));
    }

    private IEnumerator SmothlyChangeColor(Color color)
    {
        float time = 0f;

        while (time < 1)
        {
            time += Time.deltaTime;

            _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, color, time);
            
            yield return null;
        }
    }
}
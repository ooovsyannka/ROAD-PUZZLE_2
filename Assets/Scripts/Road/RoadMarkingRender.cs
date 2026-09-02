using UnityEngine;
using System.Collections;

public class RoadMarkingRender : MonoBehaviour
{
    [SerializeField] private Material _orignalMaterial;
    [SerializeField] private Material _emisionMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Coroutine _smothlyChangeMaterial;

    private void OnEnable()
    {
        _meshRenderer.material = _orignalMaterial;
    }

    public void SetOriginalMaterial()
    {
        LaunchSmothlyChangeMaterial(_orignalMaterial);
    }

    public void SetEmisionMaterila()
    {
        LaunchSmothlyChangeMaterial(_emisionMaterial);
    }

    private void LaunchSmothlyChangeMaterial(Material material)
    {
        if (_smothlyChangeMaterial != null)
            StopCoroutine(_smothlyChangeMaterial);

        _smothlyChangeMaterial = StartCoroutine(SmothlyChangeMaterial(material));
    }

    private IEnumerator SmothlyChangeMaterial(Material material)
    {
        float time = 0f;

        while (time < 1)
        {
            time += Time.deltaTime;
            _meshRenderer.material.Lerp(_meshRenderer.material, material, time);

            yield return null;
        }
    }
}

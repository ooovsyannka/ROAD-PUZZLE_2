using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private LayerMask _cellLayer;

    private Camera _camera;
    private float _maxRayDistance = 1000;
    private bool _canReadInput;

    public event Action RoadPickupAttempt;
    public event Action RoadDropAttempt;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _canReadInput = true;
    }


    private void Update()
    {
        if (_canReadInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RoadPickupAttempt?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                RoadDropAttempt?.Invoke();
            }
        }
    }

    public Ray GetRayByMousePsition() =>
       _camera.ScreenPointToRay(Input.mousePosition);

    public bool TryHitCellUnderPointer(out RaycastHit hit) =>
        Physics.Raycast(GetRayByMousePsition(), out hit, _maxRayDistance, _cellLayer);

    public void StopReadInput()=>
        _canReadInput = false;
}

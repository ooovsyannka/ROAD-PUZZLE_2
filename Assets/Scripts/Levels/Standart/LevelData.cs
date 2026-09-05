using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelIndex", order = 1)]

public partial class LevelData : ScriptableObject
{
    [SerializeField] private int _index;
    [SerializeField] private GridSection _gridSection;
    [SerializeField] private List<SingleRoadSection> _singleRoadSections;
    [SerializeField] private List<MultiRoadSection> _multiRoadSections;
    [SerializeField] private List<FinishRoadSection> _finishRoadSections;
    [SerializeField] private List<StartRoadSection> _startRoadSections;
    [SerializeField] private List<TransformSection> _streetGroundTransformSelections;
    [SerializeField] private TransformSection _cameraTransformSelection;
    [SerializeField] private HintSelection _hint;   
    [SerializeField] private TimeOfDay _timeOfDay;
    [SerializeField] private int _timeForLevelInMinute;
    [SerializeField] private float _timeForLevelInSeconds;
    [SerializeField] private int _winCoinCount;

    //private bool _isComplete;
    public bool _isComplete;

    public int Index => _index;
    public GridSection GridSection => _gridSection;
    public List<SingleRoadSection> SingleRoadSections => _singleRoadSections;
    public List<MultiRoadSection> MultiRoadSections => _multiRoadSections;
    public List<FinishRoadSection> FinishRoadSections => _finishRoadSections;
    public List<StartRoadSection> StartRoadSections => _startRoadSections;
    public List<TransformSection> StreetGroundTransformSelections => _streetGroundTransformSelections;
    public TransformSection CameraTransformSelection => _cameraTransformSelection;
    public HintSelection HintSelection => _hint;
    public TimeOfDay TimeOfDay => _timeOfDay;
    public bool IsComplete => _isComplete;
    public int TimeForLevelInMinute => _timeForLevelInMinute;
    public float TimeForLevelInSeconds => _timeForLevelInSeconds;
    public int WinCoinCount => _winCoinCount;

    public void CompleteLevel()
    {
        _isComplete = true;
    }
}

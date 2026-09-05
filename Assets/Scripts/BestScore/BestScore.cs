using UnityEngine;

public class BestScore : MonoBehaviour
{
    [SerializeField] private BestScoreRender _bestScoreRender;
    [SerializeField] private BestScoreSaver _bestScoreSaver;

    public BestScoreRender BestScoreRender => _bestScoreRender;
    public BestScoreSaver BestScoreSaver => _bestScoreSaver;
}
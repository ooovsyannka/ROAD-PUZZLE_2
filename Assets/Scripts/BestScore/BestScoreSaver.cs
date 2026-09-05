using UnityEngine;

public class BestScoreSaver : MonoBehaviour
{
    private const string BestScoreSave = nameof(BestScoreSave);


    private void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            PlayerPrefs.DeleteKey(BestScoreSave);
            print("Лучшиый результат удален ");
        }
    }

    public void SaveBestScore(int bestScore)
    {
        PlayerPrefs.SetInt(BestScoreSave, bestScore);
    }

    public int LoadBestScore() =>
         PlayerPrefs.GetInt(BestScoreSave);

}

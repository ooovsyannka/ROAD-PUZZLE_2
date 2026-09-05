using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    public List<LevelData> levelDatas;


    private const string LevelIndex = nameof(LevelIndex);
    private const int NumberCompletedLevel = 1;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (LevelData levelData in levelDatas)
            {
                PlayerPrefs.DeleteKey($"{LevelIndex} {levelData.Index}");
            }

            print("Уровни почищенны");
        }
    }

    public void SaveLevel(LevelData levelData)
    {
        PlayerPrefs.SetInt($"{LevelIndex} {levelData.Index}", NumberCompletedLevel);
    }

    public bool IsLevelComleted(LevelData levelData)
    {
        return PlayerPrefs.GetInt($"{LevelIndex} {levelData.Index}", 0) == NumberCompletedLevel;
    }
}
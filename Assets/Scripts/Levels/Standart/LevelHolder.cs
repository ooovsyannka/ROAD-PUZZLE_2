using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    [SerializeField] private List<LevelData> _levelsData;
    [SerializeField] private LevelSaver _levelSaver;

    public bool TryGetLevelBySaver(out LevelData levelData)
    {
        levelData = null;

        foreach (LevelData level in _levelsData)
        {
            if (_levelSaver.IsLevelComleted(level) == false)
            {
                levelData = level;

                return true;
            }
        }

        return false;
    }
    
    public bool TryGetLevelByIndex(out LevelData levelData)
    {
        levelData = null;

        foreach (LevelData level in _levelsData)
        {
            if (level._isComplete == false)
            {
                levelData = level;

                return true;
            }
        }

        return false;
    }
}

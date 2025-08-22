using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : Singleton<LevelsManager>
{
    [SerializeField] private LevelDataListSO _levelDataList;

    public int CurrentLevelIndex { get; set; } = 0;

    public Action OnCurrentLevelCompleted;
    public Action OnCurrentLevelFailed;

    public void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        SceneManagementService.Instance.LoadLevel(index);
    }

    public void LoadNextLevel()
    {
        if (CurrentLevelIndex >= 0 && CurrentLevelIndex < _levelDataList.levelDataList.Count - 1)
        {
            CurrentLevelIndex++;
            var levelNumber = CurrentLevelIndex + 1;
            SceneManagementService.Instance.LoadLevel(levelNumber);
        }
        else
        {
            Debug.LogWarning("No more levels to load.");
        }
    }
}
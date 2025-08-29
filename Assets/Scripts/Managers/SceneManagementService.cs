using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading and unloading of scenes
/// </summary>
public class SceneManagementService : Singleton<SceneManagementService>
{
    private LevelsManager LevelsManager => LevelsManager.Instance;

    public void LoadLevel(int index)
    {
        LevelsManager.CurrentLevelIndex = index;
        SceneManager.LoadScene($"Level_{index}");
        if (index >= 0 && index <= LevelsManager.LevelDataList.LevelDataList.Count - 1)
        {
            LevelsManager.CurrentLevelData = LevelsManager.LevelDataList.LevelDataList[index];
        }
        else
        {
            Debug.LogError($"LevelsManager: Level index {index} is out of range!");
            LevelsManager.CurrentLevelData = null;
        }
    }

    public void LoadNextLevel()
    {
        if (LevelsManager.CurrentLevelIndex >= 0 && LevelsManager.CurrentLevelIndex < LevelsManager.LevelDataList.LevelDataList.Count - 1)
        {
            var levelIndex = LevelsManager.CurrentLevelIndex + 1;
            SceneManager.LoadScene($"Level_{levelIndex}");
            LevelsManager.CurrentLevelData = LevelsManager.LevelDataList.LevelDataList[LevelsManager.CurrentLevelIndex];
        }
        else
        {
            Debug.LogWarning("No more levels to load.");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
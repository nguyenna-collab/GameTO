using System;
using System.Threading.Tasks;
using Service_Locator;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading and unloading of scenes
/// </summary>
public class SceneManagementService : Singleton<SceneManagementService>
{
    private LevelsManager LevelsManager => LevelsManager.Instance;

    public Action<string> OnLevelLoaded;
    public Action<string> OnLevelUnLoaded;

    public async Task<Task> LoadLevel(int index)
    {
        LevelsManager.CurrentLevelIndex = index;
        if (index >= 0 && index <= LevelsManager.LevelDataList.LevelDataList.Count - 1)
        {
            LevelsManager.CurrentLevelData = LevelsManager.LevelDataList.LevelDataList[index];
        }
        else
        {
            Debug.LogError($"LevelsManager: Level index {index} is out of range!");
            LevelsManager.CurrentLevelData = null;
            return Task.CompletedTask;
        }
        await LoadScene($"Level_{index}");
        ServiceLocator.Global.Get<SoundManager>(out var soundManager);
        soundManager.StopBackgroundMusic();
        return Task.CompletedTask;
    }

    public async Task LoadNextLevel()
    {
        if (LevelsManager.CurrentLevelIndex >= 0 && LevelsManager.CurrentLevelIndex < LevelsManager.LevelDataList.LevelDataList.Count - 1)
        {
            var levelIndex = LevelsManager.CurrentLevelIndex + 1;
            await LoadLevel(levelIndex);
            LevelsManager.CurrentLevelData = LevelsManager.LevelDataList.LevelDataList[LevelsManager.CurrentLevelIndex];
        }
        else
        {
            Debug.LogWarning("No more levels to load.");
        }
    }

    public async Task RestartLevel()
    {
        var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (op != null && !op.isDone)
        {
            await Task.Yield();
        }
    }

    public async Task LoadScene(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (op != null && !op.isDone)
        {
            await Task.Yield();
        }
        OnLevelLoaded?.Invoke(sceneName);
    }

    public async Task UnloadScene(string sceneName)
    {
        var op = SceneManager.UnloadSceneAsync(sceneName);
        while (op != null && !op.isDone)
        {
            await Task.Yield();
        }
        OnLevelUnLoaded?.Invoke(sceneName);
    }
}
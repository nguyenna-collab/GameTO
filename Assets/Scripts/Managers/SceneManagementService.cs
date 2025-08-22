using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading and unloading of scenes
/// </summary>
public class SceneManagementService : Singleton<SceneManagementService>
{
    /// <summary>
    /// Loads specified level scene
    /// </summary>
    /// <param name="index"></param>
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene($"Level_{index}");
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
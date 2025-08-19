using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementService : Singleton<SceneManagementService>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
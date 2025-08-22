using UnityEngine;

public class QuickSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneManagementService.Instance.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        LevelsManager.Instance.LoadNextLevel();
    }
}
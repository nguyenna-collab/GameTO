using UnityEngine;

public class QuickSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneManagementService.Instance.LoadScene(sceneName);
    }

    public void LoadNextLevelScene()
    {
        SceneManagementService.Instance.LoadNextLevel();
    }
}
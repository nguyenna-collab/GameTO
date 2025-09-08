using Service_Locator;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private SaveManager _saveManager;

        private void Start()
        {
            ServiceLocator.Global.Get<SaveManager>(out _saveManager);
        }

        private void OnApplicationQuit()
        {
            _saveManager.SaveAll();
        }
    }
}
using Service_Locator;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private UserDataManager _userDataManager;

        private void Start()
        {
            ServiceLocator.Global.Get<UserDataManager>(out _userDataManager);
        }

        private void OnApplicationQuit()
        {
            _userDataManager.Save();
        }
    }
}
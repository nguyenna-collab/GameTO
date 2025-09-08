using UnityEngine;

namespace Save
{
    /// <summary>
    /// Implements the Visitor pattern for loading data.
    /// </summary>
    public class LoadVisitor : MonoBehaviour, ILoadVisitor
    {
        private SaveManager _saveManager;

        public LoadVisitor(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public SettingsData Visit(SettingsData settingsData)
        {
            return _saveManager.LoadSettingsData(SettingsData.FILE_NAME);
        }

        public UserData Visit(UserData userData)
        {
            return _saveManager.LoadUserData(UserData.FILE_NAME);
        }
    }
}
namespace Save
{
    /// <summary>
    /// Implements the Visitor pattern for saving data.
    /// </summary>
    public class SaveVisitor : ISaveVisitor
    {
        private SaveManager _saveManager;

        public SaveVisitor(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }
        
        public void Visit(SettingsData settingsData)
        {
            _saveManager.Save(settingsData, SettingsData.FILE_NAME);
        }

        public void Visit(UserData userData)
        {
            _saveManager.Save(userData, UserData.FILE_NAME);
        }
    }
}
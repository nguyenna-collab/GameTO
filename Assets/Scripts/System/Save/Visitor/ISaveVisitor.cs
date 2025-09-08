namespace Save
{
    /// <summary>
    /// Contains Visit methods for each type of data that can be saved.
    /// </summary>
    public interface ISaveVisitor
    {
        public void Visit(SettingsData settingsData);
        public void Visit(UserData userData);
    }
    
    /// <summary>
    /// Contains Visit methods for each type of data that can be loaded.
    /// </summary>
    public interface ILoadVisitor
    {
        public SettingsData Visit(SettingsData settingsData);
        public UserData Visit(UserData userData);
    }
}
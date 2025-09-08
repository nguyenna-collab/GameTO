using Save;
using Service_Locator;
using UnityEngine;

public class SaveManager : Singleton<QuickSave>
{
    [Header("Initialize data if data is null")] 
    [SerializeField] private int _hints = 5;
    [SerializeField] private int _timeBonus = 60;
    [SerializeField] private bool _music = true;
    [SerializeField] private bool _sound = true;
    [SerializeField] private bool _vibration = true;
    
    private QuickSave _quickSave;
    private SaveVisitor _saveVisitor;
    public UserData UserData { get; set; }
    public SettingsData SettingsData { get; set; }

    public override void Awake()
    {
        base.Awake();
        ServiceLocator.Global.Register<SaveManager>(this);
        ServiceLocator.Global.Get(out _quickSave);
        _saveVisitor = new(this);
        CheckSavesIsExist();
    }
    
    private void CheckSavesIsExist()
    {
        var userFileName = UserData.FILE_NAME;
        var settingsFileName = SettingsData.FILE_NAME;
        
        UserData = LoadUserData(userFileName);
        SettingsData = LoadSettingsData(settingsFileName);
        
        if (UserData == null)
        {
            UserData = new UserData(_hints, _timeBonus);
            Save(this.UserData, userFileName);
        }
        
        if (SettingsData == null)
        {
            SettingsData = new SettingsData(_music, _sound, _vibration);
            Save(this.SettingsData, settingsFileName);
        }
    }

    public UserData LoadUserData(string fileName)
    {
        return _quickSave.Load<UserData>(fileName);
    }
    
    public SettingsData LoadSettingsData(string fileName)
    {
        return _quickSave.Load<SettingsData>(fileName);
    }

    public void Save(IData data, string fileName)
    {
        _quickSave.Save(data, fileName);
    }
    
    public void SaveUserData()
    {
        _saveVisitor.Visit(UserData);
    }
    
    public void SaveSettingsData()
    {
        _saveVisitor.Visit(SettingsData);
    }

    public void SaveAll()
    {
        _saveVisitor.Visit(SettingsData);
        _saveVisitor.Visit(UserData);
    }
    
    public void DeleteAll()
    {
        _quickSave.DeleteAll();
    }
}
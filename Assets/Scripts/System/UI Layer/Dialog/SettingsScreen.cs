using Service_Locator;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsProperties : ScreenProperties
{
    public SettingsProperties()
    {
        blockInput = true;
    }
}

public class SettingsScreen : AUIScreenController<SettingsProperties>
{
    [SerializeField] private ToggleButton _musicTogle;
    [SerializeField] private ToggleButton _soundToggle;
    [SerializeField] private ToggleButton _vibrationToggle;
    [SerializeField] private Button _closeButton;
    
    protected override void OnPropertiesSet(){}

    private SaveManager _saveManager;
    private UserData _userData;
    private SettingsData _settingsData;
    private SoundManager _soundManager;

    protected override void Awake()
    {
        ServiceLocator.Global.Get(out _saveManager);
        _userData = _saveManager.UserData;
        _settingsData = _saveManager.SettingsData;
        base.Awake();
    }

    private void Start()
    {
        UpdateView();
        SetBackgroundMusic();
    }

    private void OnEnable()
    {
        if (_soundManager == null) ServiceLocator.Global.Get(out _soundManager);
        _musicTogle.OnToggle += MusicToggle;
        _soundToggle.OnToggle += SoundToggle;
        _vibrationToggle.OnToggle += VibrationToggle;
        _closeButton.onClick.AddListener(OnClose);
        UpdateView();
        SetBackgroundMusic();
    }

    private void OnDisable()
    {
        _musicTogle.OnToggle -= MusicToggle;
        _soundToggle.OnToggle -= SoundToggle;
        _vibrationToggle.OnToggle -= VibrationToggle;
        _closeButton.onClick.RemoveListener(OnClose);
    }

    private void MusicToggle()
    {
        _settingsData.Music = _musicTogle.State;
        SetBackgroundMusic();
    }

    private void SoundToggle() => _settingsData.Sound = _soundToggle.State;
    private void VibrationToggle() => _settingsData.Vibration = _vibrationToggle.State;
    private void OnClose()
    {

        UIManager.Instance.HideDialog(ScreenID);
        SaveData();
    }

    private void SaveData()
    {
        _settingsData.Music = _musicTogle.State;
        _settingsData.Sound = _soundToggle.State;
        _settingsData.Vibration = _vibrationToggle.State;
        _saveManager.SaveSettingsData();
    }

    private void UpdateView()
    {
        bool music = _settingsData.Music;
        bool sound = _settingsData.Sound;
        bool vibration = _settingsData.Vibration;
        _musicTogle.SetState(music);
        _soundToggle.SetState(sound);
        _vibrationToggle.SetState(vibration);
    }

    private void SetBackgroundMusic()
    {
        if (_soundManager == null)
        {
            ServiceLocator.Global.Get(out _soundManager);
        }
        if (_settingsData.Music && !_soundManager.BackgroundMusic.isPlaying)
            _soundManager.PlayBackgroundMusic();
        else if(!_settingsData.Music)
            _soundManager.StopBackgroundMusic();
    }
}
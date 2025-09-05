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

    private UserDataManager _userDataManager;
    private UserData _userData;
    private SoundManager _soundManager;

    protected override void Awake()
    {
        ServiceLocator.Global.Get(out _userDataManager);
        _userData = _userDataManager.UserData;
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
        _userData.Music = _musicTogle.State;
        SetBackgroundMusic();
    }

    private void SoundToggle() => _userData.Sound = _soundToggle.State;
    private void VibrationToggle() => _userData.Vibration = _vibrationToggle.State;
    private void OnClose()
    {

        UIManager.Instance.HideDialog(ScreenID);
        SaveData();
    }

    private void SaveData()
    {
        _userData.Music = _musicTogle.State;
        _userData.Sound = _soundToggle.State;
        _userData.Vibration = _vibrationToggle.State;
        _userDataManager.Save();
    }

    private void UpdateView()
    {
        bool music = _userData.Music;
        bool sound = _userData.Sound;
        bool vibration = _userData.Vibration;
        _musicTogle.SetState(music);
        _soundToggle.SetState(sound);
        _vibrationToggle.SetState(vibration);
    }

    private void SetBackgroundMusic()
    {
        if (_soundManager == null)
        {
            Debug.Log("1" + _soundManager);
            ServiceLocator.Global.Get(out _soundManager);
            Debug.Log("2" + _soundManager);
        }
        if (_userData.Music && !_soundManager.BackgroundMusic.isPlaying)
            _soundManager.PlayBackgroundMusic();
        else if(!_userData.Music)
            _soundManager.StopBackgroundMusic();
    }
}
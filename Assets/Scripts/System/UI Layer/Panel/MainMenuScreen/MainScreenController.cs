using System;
using Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MainScreenController : AUIScreenController
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TMP_Text _hintsText;

    public override ScreenProperties BaseProperties => null;

    private SaveManager _saveManager;
    
    private void Start()
    {
        UpdateView();
    }

    private void OnEnable()
    {
        ServiceLocator.Global.Get(out _saveManager);
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        if (_saveManager.SettingsData.Music && !SoundManager.Instance.BackgroundMusic.isPlaying)
            SoundManager.Instance.PlayBackgroundMusic();
        UpdateView();
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        UIManager.Instance.ShowDialog("LevelSelection");
    }

    private void UpdateView()
    {
        _hintsText.SetText(_saveManager.UserData.Hints.ToString());
    }
}
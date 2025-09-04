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

    private UserDataManager _userDataManager;
    
    private void Start()
    {
        ServiceLocator.Global.Get(out _userDataManager);
        UpdateView();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
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
        _hintsText.SetText(_userDataManager.UserData.Hints.ToString());
    }
}
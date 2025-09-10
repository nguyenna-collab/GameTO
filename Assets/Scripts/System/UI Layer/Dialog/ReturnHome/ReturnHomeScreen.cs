using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReturnHomeScreen : AUIScreenController<ReturnHomeProperties>
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private TMP_Text _confirmText;
    [SerializeField] private TMP_Text _cancelText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    [Header("References")]
    [SerializeField] private GameplayTimer _gameplayTimer;

    private bool _isInitialized;

    protected override void Awake()
    {
        base.Awake();
        _gameplayTimer ??= FindFirstObjectByType<GameplayTimer>();
    }

    void OnEnable()
    {
        _gameplayTimer?.Pause();
        _confirmButton.onClick.AddListener(OnConfirm);
        _cancelButton.onClick.AddListener(OnCancel);
    }

    void OnDisable()
    {
        _gameplayTimer?.Resume();
        _confirmButton.onClick.RemoveListener(OnConfirm);
        _cancelButton.onClick.RemoveListener(OnCancel);
    }

    private async void OnConfirm()
    {
        try
        {
            var uiManager = UIManager.Instance;
            await SceneManagementService.Instance.LoadScene("MainMenu");
            uiManager.HidePanel("Gameplay");
            uiManager.ShowPanel("MainMenu");
            uiManager.HideDialog(ScreenID);
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    private void OnCancel()
    {
        UIManager.Instance.HideDialog(ScreenID);
    }

    protected override void OnPropertiesSet()
    {
        if (!_isInitialized)
        {
            _labelText.text = Properties.LabelText;
            _contentText.text = Properties.ContentText;
            _cancelText.text = Properties.CancelText;
            _confirmText.text = Properties.ConfirmText;
        }
    }
}
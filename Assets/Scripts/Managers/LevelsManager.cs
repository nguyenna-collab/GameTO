using System;
using UnityEngine;

/// <summary>
/// Manages all the levels in the game.
/// </summary>
public class LevelsManager : Singleton<LevelsManager>
{
    [SerializeField] private LevelDataListSO _levelDataList;

    public LevelDataSO CurrentLevelData { get; set; } = null;
    public int CurrentLevelIndex { get; set; } = 0;
    public LevelDataListSO LevelDataList => _levelDataList;

    public Action OnCurrentLevelCompleted;
    public Action OnCurrentLevelFailed;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        OnCurrentLevelCompleted += HandleLevelCompleted;
        OnCurrentLevelFailed += ShowLoseResultPanel;
    }

    void OnDestroy()
    {
        OnCurrentLevelCompleted -= HandleLevelCompleted;
        OnCurrentLevelFailed -= ShowLoseResultPanel;
    }

    private void HandleLevelCompleted()
    {
        _levelDataList.LevelDataList[CurrentLevelIndex].IsCompleted = true;
        if (CurrentLevelIndex < _levelDataList.LevelDataList.Count - 1)
            _levelDataList.LevelDataList[CurrentLevelIndex + 1].IsLocked = false;
        ShowWinResultPanel();
    }

    public void ShowWinResultPanel()
    {
        UIManager.Instance.ShowDialog("LevelResult", new LevelResultProperties(LevelsManager.Instance.CurrentLevelData.Icon, true));
    }

    public void ShowLoseResultPanel()
    {
        UIManager.Instance.ShowDialog("LevelResult", new LevelResultProperties(LevelsManager.Instance.CurrentLevelData.Icon, false));
    }
}
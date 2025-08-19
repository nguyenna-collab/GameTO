using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIDebugger : MonoBehaviour
{
    private UIManager _uiManager;

    [Header("Screen Debugging")]
    [SerializeField] private string screenIdToShow = "";
    [SerializeField] private LevelIconListSO levelIconList;

    private void Awake()
    {
        _uiManager = FindFirstObjectByType<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene. Please ensure it is present.");
            return;
        }
    }

    [Button]
    private void ShowPanelScreen()
    {
        if (_uiManager != null)
        {
            _uiManager.ShowPanel(screenIdToShow);
        }
    }

    [Button]
    private void ShowDialogScreen()
    {
        if (_uiManager != null)
        {
            _uiManager.ShowDialog(screenIdToShow);
        }
    }

    [Button]
    private void ShowLevelSelectionScreen()
    {
        if (_uiManager != null)
        {
            _uiManager.ShowPanel("LevelSelection", new LevelSelectionProperties(levelIconList));
        }
    }

    [Button]
    private void LogAllRegisteredScreens() => _uiManager?.LogAllRegisteredScreens();
}
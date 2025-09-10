using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreenController : AUIScreenController<LevelSelectionProperties>
{
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Button _backButton;

    private List<LevelIconController> _levelIconControllers = new();

    private bool _isInitialized = false;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(HandleBackButtonClick);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(HandleBackButtonClick);
    }

    private void HandleBackButtonClick()
    {
        UIManager.Instance.HideDialog(ScreenID);
    }

    protected override void OnPropertiesSet()
    {
        var levelDataListSO = Properties.levelDataList;
        var levelDataList = levelDataListSO.LevelDataList;

        if (Properties != null && _isInitialized)
        {
            for (var i = 0; i < _levelIconControllers.Count; i++)
            {
                var iconCtrl = _levelIconControllers[i];
                if (iconCtrl == null) continue;

                var iconController = iconCtrl.GetComponent<LevelIconController>();
                if (iconController != null)
                {
                    iconController.SetIconData(levelDataListSO.LevelDataList.ElementAt(i));
                }
                else
                {
                    Debug.LogWarning($"LevelIconController not found on {iconCtrl.name}");
                }
            }
        }
        else
        {
            foreach (var iconData in levelDataList)
            {
                if (iconData == null) continue;

                var iconObject = Instantiate(_iconPrefab, _iconContainer);
                var iconController = iconObject.GetComponent<LevelIconController>();
                if (iconController != null)
                {
                    iconController.SetIconData(iconData);
                }
                else
                {
                    Debug.LogWarning($"LevelIconController not found on {iconObject.name}");
                }

                _levelIconControllers.Add(iconController);
            }
            _isInitialized = true;
        }
    }
}
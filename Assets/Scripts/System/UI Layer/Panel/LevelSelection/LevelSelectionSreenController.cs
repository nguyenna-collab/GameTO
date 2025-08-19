using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreenController : AUIScreenController<LevelSelectionProperties>
{
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private Transform _iconContainer;

    private LevelIconListSO levelIconList;
    private List<LevelIconDataSO> levelIcons;

    protected override void OnPropertiesSet()
    {
        if (Properties == null) return;

        if (levelIconList == null)
        {
            levelIconList = Properties.levelIconList;
            levelIcons = levelIconList.levelIcons;
        }

        foreach (var iconData in levelIcons)
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
        }
    }
}
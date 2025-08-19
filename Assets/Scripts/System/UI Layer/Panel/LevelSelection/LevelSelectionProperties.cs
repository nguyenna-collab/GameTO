using UnityEngine;

public class LevelSelectionProperties : ScreenProperties
{
    public LevelIconListSO levelIconList;

    public LevelSelectionProperties(LevelIconListSO levelIconList) : base()
    {
        this.levelIconList = levelIconList;
    }
}
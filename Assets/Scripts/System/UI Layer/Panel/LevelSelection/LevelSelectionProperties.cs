using UnityEngine;

public class LevelSelectionProperties : ScreenProperties
{
    public LevelDataListSO levelDataList;

    public LevelSelectionProperties(LevelDataListSO levelDataList) : base()
    {
        this.levelDataList = levelDataList;
    }
}
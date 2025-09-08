using UnityEngine;

public class LevelSelectionProperties : ScreenProperties
{
    public LevelDataListSO levelDataList;

    public LevelSelectionProperties(LevelDataListSO levelDataList)
    {
        blockInput = true;
        this.levelDataList = levelDataList;
    }
}
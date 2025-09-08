using UnityEngine;

public static class ScreenPropertiesFactory
{
    public static ScreenProperties CreateSettingsProperties()
    {
        return new SettingsProperties();
    }

    public static ScreenProperties CreateLevelSelectionProperties(LevelDataListSO levelDataList)
    {
        return new LevelSelectionProperties(levelDataList);
    }

    public static ScreenProperties CreateHintProperties(string hintString)
    {
        return new HintProperties(hintString);
    }

    public static ScreenProperties CreateLevelResultProperties(Sprite sprite, bool isWin)
    {
        return new LevelResultProperties(sprite, isWin);
    }

    public static ScreenProperties CreateGameplayProperties(string descriptionString, int timeBonus, int hints)
    {
        return new GameplayProperties(descriptionString, timeBonus, hints);
    }

    public static ScreenProperties CreateReturnHomeProperties(string contentString, string labelString = "ATTENTION", string cancelString = "Continue", string confirmString = "Quit")
    {
        return new ReturnHomeProperties(contentString, labelString, cancelString, confirmString);
    }
}
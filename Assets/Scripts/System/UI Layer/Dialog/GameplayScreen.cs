using TMPro;
using UnityEngine;

public class GameplayProperties : ScreenProperties
{
    public string DescriptionString { get; private set; }
    public int TimeBonus { get; private set; }
    public int Hints { get; private set; }
    
    public GameplayProperties(string descriptionString, int timeBonus, int hints)
    {
        DescriptionString = descriptionString;
        TimeBonus = timeBonus;
        Hints = hints;
    }
}

public class GameplayScreen : AUIScreenController<GameplayProperties>
{
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TimeBonus _timeBonus;
    [SerializeField] private Hint _hint;

    protected override void OnPropertiesSet()
    {
        _descriptionText.text = Properties.DescriptionString;
        _timeBonus.BonusAmount = Properties.TimeBonus;
        _hint.HintAmount = Properties.Hints;
    }
}
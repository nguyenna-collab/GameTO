using TMPro;
using UnityEngine;

public class GameplayProperties : ScreenProperties
{
    public GameplayProperties(string descriptionText)
    {
        DescriptionText = descriptionText;
    }
    public string DescriptionText { get; set; }
}

public class GameplayScreen : AUIScreenController<GameplayProperties>
{
    [SerializeField] private TMP_Text _descriptionText;

    protected override void OnPropertiesSet()
    {
        _descriptionText.text = Properties.DescriptionText;
    }
}
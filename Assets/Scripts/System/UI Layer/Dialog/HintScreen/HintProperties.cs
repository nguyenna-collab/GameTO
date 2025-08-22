using UnityEngine;

public class HintProperties : ScreenProperties
{
    public HintProperties(string hintText)
    {
        HintText = hintText;
    }

    public string HintText { get; set; }
}
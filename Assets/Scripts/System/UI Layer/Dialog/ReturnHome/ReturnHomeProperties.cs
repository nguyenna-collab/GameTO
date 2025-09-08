using UnityEngine;

public class ReturnHomeProperties : ScreenProperties
{
    public ReturnHomeProperties(string contentText, string labelText, string cancelText, string confirmText)
    {
        LabelText = labelText;
        ContentText = contentText;
        CancelText = cancelText;
        ConfirmText = confirmText;
    }

    public string LabelText { get; set; }
    public string ContentText { get; set; }
    public string CancelText { get; set; }
    public string ConfirmText { get; set; }
}
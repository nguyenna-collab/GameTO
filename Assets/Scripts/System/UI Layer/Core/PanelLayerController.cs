using UnityEngine;

public class PanelLayerController : AUILayerController
{
    public override void HideAll()
    {
        foreach (var screen in screens.Values)
        {
            screen.Hide();
        }
    }
}
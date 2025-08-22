using UnityEngine;

public class DialogLayerController : AUILayerController
{
    public override void HideAll()
    {
        foreach (var screen in screens.Values)
        {
            screen.Hide();
        }
    }
}
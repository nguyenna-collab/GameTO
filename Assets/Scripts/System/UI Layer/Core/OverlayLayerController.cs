using UnityEngine;

public class OverlayLayerController : AUILayerController
{
    [SerializeField] private GameObject blockingOverlay;
    public void ShowBlockingOverlay()
    {
        if (blockingOverlay != null)
        {
            blockingOverlay.SetActive(true);
        }
    }

    public void HideBlockingOverlay()
    {
        if (blockingOverlay != null)
        {
            blockingOverlay.SetActive(false);
        }
    }

    public override void HideScreen(string screenId)
    {
        throw new System.NotImplementedException();
    }
}

using UnityEngine;

public static class ParentSetter
{
    public static void SetParent(this Transform transform, Transform oldCanvas, int siblingIndex = 0)
    {
        transform.SetParent(oldCanvas.transform, true);
        transform.SetSiblingIndex(siblingIndex);
    }
}
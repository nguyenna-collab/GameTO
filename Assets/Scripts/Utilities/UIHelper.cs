using UnityEngine;
using UnityEngine.EventSystems;

public static class UIHelper
{
    public static bool IsTouchingUI(PointerEventData e, RectTransform rect, out Vector3 worldPosition)
    {
        worldPosition = Vector3.zero;
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rect,
            e.position,
            e.pressEventCamera,
            out worldPosition);
    }
}

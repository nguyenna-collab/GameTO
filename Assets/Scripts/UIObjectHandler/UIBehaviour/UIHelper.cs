using UnityEngine;
using UnityEngine.EventSystems;

public static class UIHelper
{
    public static bool IsTouchingUI(PointerEventData e, MonoBehaviour mono, out Vector3 worldPosition)
    {
        worldPosition = Vector3.zero;
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(
            mono.GetComponent<RectTransform>(),
            e.position,
            e.pressEventCamera,
            out worldPosition);
    }

    public static bool IsOverlapUI(this RectTransform rect, RectTransform target)
    {
        Bounds rectBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rect);
        Bounds targetBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(target);
        return rectBounds.Intersects(targetBounds);
    }
}
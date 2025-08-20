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
        //Convert RectTransform to World Space because rect and target may be in different parents
        Vector3[] rectCorners = new Vector3[4];
        rect.GetWorldCorners(rectCorners);
        Bounds rectBounds = new Bounds(rectCorners[0], Vector3.zero);
        for (int i = 1; i < 4; i++)
        {
            rectBounds.Encapsulate(rectCorners[i]);
        }

        Vector3[] targetCorners = new Vector3[4];
        target.GetWorldCorners(targetCorners);
        Bounds targetBounds = new Bounds(targetCorners[0], Vector3.zero);
        for (int i = 1; i < 4; i++)
        {
            targetBounds.Encapsulate(targetCorners[i]);
        }

        return rectBounds.Intersects(targetBounds);
    }
}

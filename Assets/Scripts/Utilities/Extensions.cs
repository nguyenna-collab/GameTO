using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    /// <summary>
    /// Check if the given RectTransform overlaps with the target RectTransform.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="target"></param>
    /// <param name="screenPosition">Touch position</param>
    /// <returns></returns>
    public static bool IsOverlapUI(this RectTransform rect, RectTransform target, Vector2 screenPosition)
    {
        // Convert RectTransform to World Space because rect and target may be in different parents
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

        if (!rectBounds.Intersects(targetBounds))
        {
            return false;
        }

        // Raycast to check if any UI blocks the target at the touch position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            var resultRect = result.gameObject.GetComponent<RectTransform>();
            if (resultRect == null) continue;
            if (resultRect == target)
            {
                break; // Target is reached, no UI blocks above
            }
            // If have other UI blocks above the target, return false
            if (resultRect != rect && resultRect != target)
            {
                return false;
            }
        }
        return true;
    }

    public static void FlipX(this Transform transform)
    {
        var localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public static void SetParent(this Transform transform, Transform oldCanvas, int siblingIndex = 0)
    {
        transform.SetParent(oldCanvas, true);
        transform.SetSiblingIndex(siblingIndex);
    }
}
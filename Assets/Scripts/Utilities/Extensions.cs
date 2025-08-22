using UnityEngine;

public static class Extensions
{
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
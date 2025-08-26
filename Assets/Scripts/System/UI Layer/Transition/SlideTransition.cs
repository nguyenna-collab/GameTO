using UnityEngine;
using System;

[RequireComponent(typeof(RectTransform))]
public class SlideTransition : ATransitionComponent
{
    public enum SlideDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private float duration = 0.3f;
    [SerializeField] private SlideDirection direction = SlideDirection.Left;
    [SerializeField] private bool isSlideIn = true;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public override void Animate(Transform target, Action onAnimationComplete)
    {
        if (rectTransform == null) return;

        // Calculate start and end positions
        Vector2 startPos, endPos;
        
        if (isSlideIn)
        {
            startPos = GetOffscreenPosition();
            endPos = originalPosition;
        }
        else
        {
            startPos = originalPosition;
            endPos = GetOffscreenPosition();
        }

        // Set initial position
        rectTransform.anchoredPosition = startPos;

        // Start animation
        StartCoroutine(Slide(endPos, onAnimationComplete));
    }

    private Vector2 GetOffscreenPosition()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 canvasSize = rectTransform.sizeDelta;
        
        switch (direction)
        {
            case SlideDirection.Left:
                return new Vector2(-screenSize.x, originalPosition.y);
            case SlideDirection.Right:
                return new Vector2(screenSize.x, originalPosition.y);
            case SlideDirection.Up:
                return new Vector2(originalPosition.x, screenSize.y);
            case SlideDirection.Down:
                return new Vector2(originalPosition.x, -screenSize.y);
            default:
                return originalPosition;
        }
    }

    private System.Collections.IEnumerator Slide(Vector2 endPosition, Action onAnimationComplete)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float time = 0;

        while (time < duration)
        {
            float progress = time / duration;
            float curveValue = curve.Evaluate(progress);

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, curveValue);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
        onAnimationComplete?.Invoke();
        OnTransitionComplete?.Invoke();
    }
}
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class FadeTransition : ATransitionComponent
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private bool isFadeIn;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        canvasGroup.alpha = 1f;
    }

    public override void Animate(Transform target, Action onAnimationComplete)
    {
        float startAlpha = isFadeIn ? 0f : 1f;
        float endAlpha = isFadeIn ? 1f : 0f;

        canvasGroup.alpha = startAlpha;
        StartCoroutine(Fade(endAlpha, onAnimationComplete));
    }

    private System.Collections.IEnumerator Fade(float endAlpha, Action onAnimationComplete)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onAnimationComplete?.Invoke();
    }
}
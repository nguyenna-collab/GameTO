using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScaleTransition : ATransitionComponent
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private bool isScaleIn = true;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        canvasGroup.alpha = 1f;
    }

    public override void Animate(Transform target, Action onComplete = null)
    {
        Vector3 startScale = isScaleIn ? Vector3.zero : Vector3.one;
        Vector3 endScale = isScaleIn ? Vector3.one : Vector3.zero;

        target.localScale = startScale;
        StartCoroutine(Scale(target, endScale, onComplete));
    }

    private System.Collections.IEnumerator Scale(Transform target, Vector3 endScale, Action onComplete)
    {
        float time = 0;
        Vector3 startScale = target.localScale;

        while (time < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        target.localScale = endScale;
        onComplete?.Invoke();
        OnTransitionComplete?.Invoke();
    }
}
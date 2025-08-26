using UnityEngine;
using UnityEngine.Events;

public abstract class ATransitionComponent : MonoBehaviour {
    public UnityEvent OnTransitionComplete;
    public abstract void Animate(Transform target, System.Action onComplete = null);
}
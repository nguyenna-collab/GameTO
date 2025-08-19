using UnityEngine;

public abstract class ATransitionComponent : MonoBehaviour {
    public abstract void Animate(Transform target, System.Action onComplete = null);
}
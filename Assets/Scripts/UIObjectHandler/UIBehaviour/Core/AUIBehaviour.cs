using UnityEngine;

public abstract class AUIBehaviour : MonoBehaviour
{
    [SerializeField] protected RectTransform _targetRect;

    protected virtual bool IsTouchingTarget()
    {
        return GetComponent<RectTransform>().IsOverlapUI(_targetRect);
    }
}
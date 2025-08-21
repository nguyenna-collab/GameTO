using UnityEngine;

public abstract class AUIBehaviour : MonoBehaviour
{
    [SerializeField] protected RectTransform _targetRect;
    
    public Vector3 OriginalPosition { get; private set; }
    
    protected Transform _initialParent;
    protected int _initialSiblingIndex;

    protected virtual void OnEnable()
    {
        _initialParent = transform.parent;
        _initialSiblingIndex = transform.GetSiblingIndex();
        OriginalPosition = transform.position;
    }
    
    protected virtual bool IsTouchingTarget()
    {
        return GetComponent<RectTransform>().IsOverlapUI(_targetRect);
    }
    
    protected abstract void CompleteObjective();
    protected abstract void FailObjective();
}
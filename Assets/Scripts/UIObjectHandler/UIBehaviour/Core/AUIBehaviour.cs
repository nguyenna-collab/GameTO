using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AUIBehaviour : MonoBehaviour
{
    [SerializeField] protected RectTransform _targetRect;

    public Vector3 OriginalPosition { get; private set; }
    public bool CanDetectTarget { get; set; } = true;

    protected Transform _initialParent;
    protected int _initialSiblingIndex;

    protected virtual void OnEnable()
    {
        _initialParent = transform.parent;
        _initialSiblingIndex = transform.GetSiblingIndex();
        OriginalPosition = transform.position;
    }

    protected virtual bool IsTouchingTarget(PointerEventData eventData)
    {
        if (!CanDetectTarget) return false;
        return GetComponent<RectTransform>().IsOverlapUI(_targetRect, eventData.position);
    }
}
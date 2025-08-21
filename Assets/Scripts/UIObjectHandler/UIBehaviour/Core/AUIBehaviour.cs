using UnityEngine;

[RequireComponent(typeof(DraggableUI))]
public abstract class AUIBehaviour : MonoBehaviour
{
    [SerializeField] protected RectTransform _targetRect;
    [SerializeField] private Transform _dragUICanvas;
    
    protected DraggableUI _draggableUI;
    protected int _initialSiblingIndex;
    
    protected virtual void OnEnable()
    {
        _initialSiblingIndex = transform.GetSiblingIndex();
        
        _draggableUI = GetComponent<DraggableUI>();
        
        _draggableUI.OnBeginDragEvent.AddListener(OnBeginDrag);
        _draggableUI.OnEndDragEvent.AddListener(OnEndDrag);
    }

    protected virtual void OnDisable()
    {
        _draggableUI.OnBeginDragEvent.RemoveListener(OnBeginDrag);
        _draggableUI.OnEndDragEvent.RemoveListener(OnEndDrag);
    }
    
    protected virtual bool IsTouchingTarget()
    {
        return GetComponent<RectTransform>().IsOverlapUI(_targetRect);
    }
    
    protected abstract void CompleteObjective();
    protected abstract void FailObjective();
    
    protected virtual void OnBeginDrag()
    {
        transform.SetParent(_dragUICanvas);
    }

    protected virtual void OnEndDrag()
    {
        transform.SetParent(_dragUICanvas, _initialSiblingIndex);
    }
}
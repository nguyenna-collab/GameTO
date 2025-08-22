using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDropTarget
{
    void OnDropReceived(DraggableUI draggable, PointerEventData eventData);
}


[RequireComponent(typeof(Image))]
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _dragUICanvas;
    [SerializeField] private bool _resetSizeOnDrag = true;
    [SerializeField] private bool _resetSizeOnDrop = true;

    public UnityEvent OnBeginDragEvent;
    public UnityEvent OnDragEvent;
    public UnityEvent<PointerEventData> OnDropped;

    public int InitialSiblingOrder { get; private set; }
    public Vector3 InitialPosition { get; private set; }
    public Transform InitialParent { get; private set; }

    private Vector3 _distToTouchPosition;
    private Image _image;

    private bool _isAwaked;

    private void OnEnable()
    {
        if (!_isAwaked)
        {
            _isAwaked = true;
            _image = GetComponent<Image>();
            _dragUICanvas = GetComponentInParent<Canvas>();
            CacheInitialState();
        }
    }

    private void CacheInitialState()
    {
        InitialParent = transform.parent;
        InitialSiblingOrder = transform.GetSiblingIndex();
        InitialPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
            return;

        _distToTouchPosition = transform.position - worldPosition;
        if (_resetSizeOnDrag) _image.SetNativeSize();

        transform.SetParent(_dragUICanvas.transform, true);
        OnBeginDragEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            transform.position = worldPosition + _distToTouchPosition;
            OnDragEvent?.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDropped?.Invoke(eventData);
    }

    public void RestoreToInitial()
    {
        transform.SetParent(InitialParent, true);
        transform.position = InitialPosition;
        transform.SetSiblingIndex(InitialSiblingOrder);
        if (_resetSizeOnDrop) _image.SetNativeSize();
    }
}

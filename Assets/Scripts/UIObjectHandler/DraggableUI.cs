using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _dragUICanvas;

    [Header("Events")]
    public UnityEvent OnBeginDragEvent;
    public UnityEvent OnDragEvent;
    public UnityEvent OnEndDragEvent;
    [SerializeField] private bool _disableBeginDragEvent;
    [SerializeField] private bool _disableEndDragEvent;

    // == Init Data ==
    public int InitialSiblingOrder { get; private set; }
    public Canvas DragUICanvas { get => _dragUICanvas; }

    private Vector3 _distToTouchPosition;
    private Image _image;
    private Transform _parent;

    #region Unity Callbacks

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        _parent = transform.parent;
        InitialSiblingOrder = transform.GetSiblingIndex();

        OnBeginDragEvent.AddListener(HandleBeginDrag);
        OnDragEvent.AddListener(HandleDrag);
        OnEndDragEvent.AddListener(HandleEndDrag);
    }

    private void OnDisable()
    {
        OnBeginDragEvent.RemoveListener(HandleBeginDrag);
        OnDragEvent.RemoveListener(HandleDrag);
        OnEndDragEvent.RemoveListener(HandleEndDrag);
    }
    #endregion

    #region Drag Event Methods
    private void HandleBeginDrag()
    {
        if (_disableBeginDragEvent)
            return;
        _image.SetNativeSize();
        transform.SetParent(_dragUICanvas.transform);
    }

    private void HandleDrag()
    {
        // Handle the drag event
    }

    private void HandleEndDrag()
    {
        if (_disableEndDragEvent)
            return;
        transform.SetParent(_parent);
        transform.SetSiblingIndex(InitialSiblingOrder);
        _image.SetNativeSize();
    }

    // Implement interface
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            _distToTouchPosition = transform.position - worldPosition;
            OnBeginDragEvent?.Invoke();
        }
    }

    // Implement interface
    public void OnDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            transform.position = worldPosition + _distToTouchPosition;
            //TODO: following touch finger smoothly - Lerp
            OnDragEvent?.Invoke();
        }
    }

    // Implement interface
    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke();
    }
    #endregion
}
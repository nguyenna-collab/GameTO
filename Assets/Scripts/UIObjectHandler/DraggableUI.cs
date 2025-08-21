using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _dragUICanvas;
    [SerializeField] private RectTransform _mainCharacter;


    [Header("Events")]
    public UnityEvent OnBeginDragEvent;
    public UnityEvent OnDragEvent;
    public UnityEvent OnEndDragEvent;

    // == Init Data ==
    public Vector3 OriginalPosition { get; private set; }
    public int InitialSiblingOrder { get; private set; }
    public Canvas DragUICanvas { get => _dragUICanvas; }

    private Vector3 _distToTouchPosition;
    private Image _image;
    private Transform _parent;

    #region Unity Callbacks
    void Awake()
    {
        _parent = transform.parent;
        InitialSiblingOrder = transform.GetSiblingIndex();
        _image = GetComponent<Image>();
        OriginalPosition = transform.position;
    }

    private void OnEnable()
    {
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
        _image.preserveAspect = true;
    }

    private void HandleDrag()
    {
        // Handle the drag event
    }

    private void HandleEndDrag()
    {
        transform.SetParent(_parent, true);
        transform.SetSiblingIndex(InitialSiblingOrder);
        _image.preserveAspect = true;
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
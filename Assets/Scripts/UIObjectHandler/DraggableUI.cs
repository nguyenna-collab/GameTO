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
    [SerializeField] private UnityEvent _onBeginDrag;
    [SerializeField] private UnityEvent _onDrag;
    [SerializeField] private UnityEvent _onEndDrag;


    // == Init Data ==
    private Image _image;
    private Transform _parent;
    private Vector3 _originalPosition;
    private int _siblingOrder;

    private Vector3 _distToTouchPosition;

    #region Unity Callbacks
    void Awake()
    {
        _parent = transform.parent;
        _siblingOrder = transform.GetSiblingIndex();
        _image = GetComponent<Image>();
        _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        _onBeginDrag.AddListener(HandleBeginDrag);
        _onDrag.AddListener(HandleDrag);
        _onEndDrag.AddListener(HandleEndDrag);
    }

    private void OnDisable()
    {
        _onBeginDrag.RemoveListener(HandleBeginDrag);
        _onDrag.RemoveListener(HandleDrag);
        _onEndDrag.RemoveListener(HandleEndDrag);
    }
    #endregion

    #region Drag Event Methods
    private void HandleBeginDrag()
    {
        transform.SetParent(_dragUICanvas.transform, true);
        _image.preserveAspect = true;
    }

    private void HandleDrag()
    {
        // Handle the drag event
    }

    private void HandleEndDrag()
    {
        transform.SetParent(_parent, true);
        transform.SetSiblingIndex(_siblingOrder);
        _image.preserveAspect = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            _distToTouchPosition = transform.position - worldPosition;
            _onBeginDrag?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            transform.position = worldPosition + _distToTouchPosition;
            //TODO: following touch finger smoothly - Lerp
            _onDrag?.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UIHelper.IsTouchingUI(eventData, this, out Vector3 worldPosition))
        {
            if (GetComponent<RectTransform>().IsOverlapUI(_mainCharacter))
            {
                Debug.Log("Snapped to character position");
                transform.position = _mainCharacter.position;
            }
            else
            {
                Debug.Log("Snapped back to original position");
                transform.position = _originalPosition;
            }
        }
    }
    #endregion
}
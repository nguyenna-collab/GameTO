using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class DragAndReplaceUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Sprite _replacmentSprite;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private DraggableUI _draggableUI;

    private Image _image;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _image =  GetComponent<Image>();
        _draggableUI.OnDropped.AddListener(SnapToFace);
    }
    
    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(SnapToFace);
    }

    private void SnapToFace(PointerEventData eventData)
    {
        if (IsTouchingTarget())
            OnDropReceived(_draggableUI, eventData);
        else
            FailObjective();
    }

    protected override void CompleteObjective()
    {
        transform.SetParent(_targetPosition.parent, true);
        transform.position = _targetPosition.position;

        _image.sprite = _replacmentSprite;
        _image.SetNativeSize();
    
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objectiveSO?.FailObjective();
    }

    public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
    {
        if (_targetPosition == null)
        {
            FailObjective();
            return;
        }

        CompleteObjective();
    }
}
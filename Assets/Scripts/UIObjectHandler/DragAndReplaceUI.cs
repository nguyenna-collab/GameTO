using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class DragAndReplaceUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private bool _snapToTargetPosition = true;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Image _imageToReplace;
    [SerializeField] private Sprite _replacmentSprite;
    [SerializeField] private Objective _objective;

    private DraggableUI _draggableUI;

    private Image _image;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _image = GetComponent<Image>();
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

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }

    public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
    {
        if (_snapToTargetPosition)
        {
            transform.SetParent(_targetPosition.parent, true);
            transform.position = _targetPosition.position;
        }

        _imageToReplace.sprite = _replacmentSprite;
        _imageToReplace.SetNativeSize();
    
        _objective?.CompleteObjective();
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndRepositionUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private DraggableUI _draggableUI;
    private Image _image;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _image = GetComponent<Image>();

        _draggableUI.OnDropped.AddListener(HandleDrop);
    }

    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(HandleDrop);
    }

    private void HandleDrop(PointerEventData eventData)
    {
        if (IsTouchingTarget())
            OnDropReceived(_draggableUI, eventData);
        else
            FailObjective();
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

    protected override void CompleteObjective()
    {
        transform.SetParent(_targetPosition, true);
        transform.position = _targetPosition.position;
        _objectiveSO?.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objectiveSO?.FailObjective();
    }
}

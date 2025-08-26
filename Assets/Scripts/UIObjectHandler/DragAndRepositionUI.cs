using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndRepositionUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Objective _objective;
    [SerializeField] private AudioClip _clipWhenDroppedOnTarget;

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

    private void CompleteObjective()
    {
        if (_clipWhenDroppedOnTarget != null)
        {
            SoundManager.Instance.PlaySFX(_clipWhenDroppedOnTarget);
        }
        transform.SetParent(_targetPosition, true);
        transform.position = _targetPosition.position;
        _objective?.CompleteObjective();
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }
}

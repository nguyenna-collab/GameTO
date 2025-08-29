using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndRepositionUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Objective _objective;
    
    [Header("When Completed")]
    [SerializeField] private AudioClip _clipWhenDroppedOnTarget;
    [SerializeField] private bool _resetLocalRotation = false;
    public UnityEvent OnSuccess;

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
        if (IsTouchingTarget(eventData))
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
            SoundManager.Instance.PlaySFX(_clipWhenDroppedOnTarget);
        transform.SetParent(_targetPosition, true);
        if (_resetLocalRotation)
            transform.localEulerAngles = Vector3.zero;
        transform.position = _targetPosition.position;
        _draggableUI.enabled = false;
        _objective?.CompleteObjective();
        OnSuccess?.Invoke();
        this.enabled = false;
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }
}

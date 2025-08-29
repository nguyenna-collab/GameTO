using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DraggableUI))]
public class DragAndSetActiveUI : AUIBehaviour, IDropTarget
{
    [SerializeField] private GameObject _disableUI;
    [SerializeField] private GameObject _enableUI;
    [SerializeField] private Objective _objective;
    [SerializeField] private AudioClip _successSound;
    [SerializeField] private UnityEvent _onSuccess;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(DropOnTarget);
    }

    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(DropOnTarget);
    }

    private void DropOnTarget(PointerEventData eventData)
    {
        if (IsTouchingTarget(eventData))
            OnDropReceived(_draggableUI, eventData);
        else
            FailObjective();
    }

    public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
    {
        if (_successSound != null)
            SoundManager.Instance.PlaySFX(_successSound, default, 0.5f);
        _objective.CompleteObjective();
        _onSuccess?.Invoke();
        if (_enableUI) _enableUI.SetActive(true);
        if (_disableUI) _disableUI.SetActive(false);
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }
}
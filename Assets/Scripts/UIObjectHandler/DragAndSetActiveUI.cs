using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DraggableUI))]
public class DragAndSetActiveUI : AUIBehaviour
{
    [SerializeField] private GameObject _disableUI;
    [SerializeField] private GameObject _enableUI;
    [SerializeField] private Objective _objective;
    [SerializeField] private AudioClip _successSound;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(SnapToFace);
    }

    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(SnapToFace);
    }

    private void SnapToFace(PointerEventData eventData)
    {
        if (IsTouchingTarget())
        {
            CompleteObjective();
        }
        else
        {
            FailObjective();
        }
    }

    private void CompleteObjective()
    {
        _enableUI.SetActive(true);
        _disableUI.SetActive(false);
        _objective.CompleteObjective();
        if (_successSound != null)
           SoundManager.Instance.PlaySFX(_successSound, default, 0.5f);
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }
}
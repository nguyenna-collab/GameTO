using System;
using GameSystemsCookbook;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class DragAndSetActiveUI : AUIBehaviour
{
    [SerializeField] private GameObject _disableUI;
    [SerializeField] private GameObject _enableUI;
    [SerializeField] private RestorePositionSO _restorePositionSO;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnEndDragEvent.AddListener(SnapToFace);
    }

    protected void OnDisable()
    {
        _draggableUI.OnEndDragEvent.RemoveListener(SnapToFace);
    }

    private void SnapToFace()
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

    protected override void CompleteObjective()
    {
        _objectiveSO.CompleteObjective();
        _enableUI.SetActive(true);
        _disableUI.SetActive(false);
    }

    protected override void FailObjective()
    {
        _restorePositionSO.ApplyBehaviour(transform, OriginalPosition);
        transform.SetParent(_initialParent, _draggableUI.InitialSiblingOrder);
        _objectiveSO.FailObjective();
    }
}
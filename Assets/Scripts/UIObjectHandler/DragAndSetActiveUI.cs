using System;
using GameSystemsCookbook;
using UnityEngine;
using UnityEngine.EventSystems;
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

    protected override void CompleteObjective()
    {
        _enableUI.SetActive(true);
        _disableUI.SetActive(false);
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objectiveSO.FailObjective();
    }
}
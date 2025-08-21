using System;
using GameSystemsCookbook;
using UnityEngine;

public class Eye : AUIBehaviour
{
    [SerializeField] private Transform _eyePosition;
    [SerializeField] private RestorePositionSO _restorePositionSO;
    [SerializeField] private ObjectiveSO _objectiveSO;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI.OnEndDragEvent.AddListener(SnapToFace);
    }

    protected override void OnDisable()
    {
        _draggableUI.OnEndDragEvent.RemoveListener(SnapToFace);
    }

    public void SnapToFace()
    {
        if (IsTouchingTarget())
        {
            if (_eyePosition != null)
            {
                CompleteObjective();
            }
        }
        else
        {
            FailObjective();
        }
    }

    protected override void CompleteObjective()
    {
        transform.position = _eyePosition.position;
        transform.SetParent(_eyePosition.parent, true);
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _restorePositionSO.ApplyBehaviour(transform, _draggableUI.OriginalPosition);
        transform.SetParent(_draggableUI.DragUICanvas.transform, _draggableUI.InitialSiblingOrder);
        _objectiveSO.FailObjective();
    }
}
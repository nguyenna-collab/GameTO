using UnityEngine;

[RequireComponent(typeof(DraggableUI))]
public class Eye : AUIBehaviour
{
    [SerializeField] private Transform _eyePosition;
    [SerializeField] private RestorePositionSO _restorePositionSO;

    private DraggableUI _draggableUI;

    void Awake()
    {
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnEndDragEvent.AddListener(SnapToFace);
    }

    private void OnDestroy()
    {
        _draggableUI.OnEndDragEvent.RemoveListener(SnapToFace);
    }

    public void SnapToFace()
    {
        if (IsTouchingTarget())
        {
            if (_eyePosition != null)
            {
                transform.position = _eyePosition.position;
                transform.SetParent(_eyePosition.parent, true);
            }
        }
        else
        {
            _restorePositionSO.ApplyBehaviour(transform, _draggableUI.OriginalPosition);
        }
    }
}
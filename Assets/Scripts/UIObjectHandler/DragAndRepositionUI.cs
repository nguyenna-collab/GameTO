using UnityEngine;
using UnityEngine.UI;

public class DragAndRepositionUI : AUIBehaviour
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private RestorePositionSO _restorePositionSO;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private DraggableUI _draggableUI;
    private Image _image;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _image = GetComponent<Image>();
        _draggableUI.OnEndDragEvent.AddListener(SnapToFace);
    }

    protected void OnDisable()
    {
        _draggableUI.OnEndDragEvent.RemoveListener(SnapToFace);
    }

    public void SnapToFace()
    {
        if (IsTouchingTarget())
        {
            if (_targetPosition != null)
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
        transform.SetParent(_targetPosition);
        transform.position = _targetPosition.position;
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        transform.SetParent(_initialParent);
        transform.SetSiblingIndex(_initialSiblingIndex);
        transform.position = OriginalPosition;
        _image.SetNativeSize();
        _objectiveSO.FailObjective();
    }
}
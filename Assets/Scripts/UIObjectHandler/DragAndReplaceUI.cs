using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class DragAndReplaceUI : AUIBehaviour
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Sprite _replacmentSprite;
    [SerializeField] private RestorePositionSO _restorePositionSO;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private DraggableUI _draggableUI;

    private Image _image;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _image =  GetComponent<Image>();
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
        _image.sprite = _replacmentSprite;
        _image.SetNativeSize();
        transform.position = _targetPosition.position;
        transform.SetParent(_targetPosition.parent, true);
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _restorePositionSO.ApplyBehaviour(transform, OriginalPosition);
        transform.SetParent(_initialParent, _draggableUI.InitialSiblingOrder);
        _objectiveSO.FailObjective();
    }
}
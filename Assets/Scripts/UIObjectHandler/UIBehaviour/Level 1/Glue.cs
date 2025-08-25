using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Glue : AUIBehaviour, IDropTarget {
    [SerializeField] private Objective _objective;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private Image _glueLineImage;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(TriggerAnimation);
    }

    public void TriggerAnimation(PointerEventData eventData)
    {
        if (IsTouchingTarget())
            OnDropReceived(_draggableUI, eventData);
        else
            FailObjective();
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }

    public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() =>
        {
            transform.position = _startPos.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30));
        });
        s.Join(transform.DOMove(_endPos.position, 1f));
        s.Join(_glueLineImage.DOFillAmount(1f, 1f));
        s.AppendCallback(() =>
        {
            _objective?.CompleteObjective();
            gameObject.SetActive(false);
        });
    }
}
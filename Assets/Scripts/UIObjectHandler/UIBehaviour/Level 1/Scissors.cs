using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class Scissors : AUIBehaviour
{
    [Header("Scissors")]
    [SerializeField] private Transform _trimPosition;
    [SerializeField] private Transform _leftPart;
    [SerializeField] private Transform _rightPart;
    [SerializeField, Range(1, 30)] private int _amplitude;
    
    [Header("Dog")]
    [SerializeField] private Image _dogImage;
    [SerializeField] private Sprite _dogAfterTrimImage;
    [SerializeField] private GameObject _trimmedFur;
    
    [Space(20)]
    [SerializeField] private RestorePositionSO _restorePositionSO;

    private DraggableUI _draggableUI;
    
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(HandleDrop);
    }

    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(HandleDrop);
    }

    private void HandleDrop(PointerEventData eventData)
    {
        if (IsTouchingTarget())
            CompleteObjective();
        else
            FailObjective();
    }

    protected override void CompleteObjective()
    {
        TrimFur();
    }

    protected override void FailObjective()
    {
        _draggableUI.RestoreToInitial();
    }

    private Sequence TrimFur()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => transform.position = _trimPosition.position);
        sequence.Join(_leftPart.DORotate(new Vector3(0, 0, -_amplitude), .25f)).SetLoops(2, LoopType.Yoyo);
        sequence.Join(_rightPart.DORotate(new Vector3(0, 0, _amplitude), .25f)).SetLoops(2, LoopType.Yoyo);
        sequence.AppendCallback(() =>
        {
            _dogImage.sprite = _dogAfterTrimImage;
            _dogImage.preserveAspect = true;
            gameObject.SetActive(false);
            _trimmedFur.SetActive(true);
        });
        
        return sequence;
    }
}

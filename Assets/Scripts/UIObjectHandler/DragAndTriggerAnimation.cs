using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragAndTriggerAnimation : AUIBehaviour, IDropTarget
{
    [SerializeField] private string _animationTriggerName;
    [SerializeField] private Animator _animator;
    [SerializeField] private Objective _objective;
    [SerializeField] private bool _disableOnEnter;
    [SerializeField] private bool _disableWhileCompleted;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(TriggerAnimation);
        _animator.enabled = false;
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
        if (_animator != null)
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                if (_disableOnEnter)
                    GetComponent<Image>().enabled = false;
                _animator.enabled = true;
                _animator.SetTrigger(_animationTriggerName);
            }).AppendInterval(_animator.GetCurrentAnimatorStateInfo(0).length);
            s.AppendCallback(() =>
            {
                _objective?.CompleteObjective();
                _animator.enabled = false;
                if (_disableWhileCompleted)
                    gameObject.SetActive(false);
            });
        }
    }
}
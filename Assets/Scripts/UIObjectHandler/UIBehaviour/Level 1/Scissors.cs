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
    
    [Header("Dog")]
    [SerializeField] private Transform _dog;
    [SerializeField] private GameObject _trimmedFur;

    private DraggableUI _draggableUI;
    private Animator _anim;
    
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(HandleDrop);
        _anim = GetComponent<Animator>();
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

    private void CompleteObjective()
    {
        TrimFur();
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
    }

    private Sequence TrimFur()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            transform.position = _trimPosition.position;
            _anim.enabled = true;
            _anim.SetTrigger("Trim");
            _dog.GetComponent<Animator>().SetTrigger("Trim");
        }).AppendInterval(_anim.GetCurrentAnimatorStateInfo(0).length);
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
            _trimmedFur.SetActive(true);
        });
        
        return sequence;
    }
}

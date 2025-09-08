using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Glue : AUIBehaviour, IDropTarget
{
    [SerializeField] private Objective _objective;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private Image _glueLineImage;
    [SerializeField] private AudioClip _glueSound;

    private DraggableUI _draggableUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _draggableUI.OnDropped.AddListener(TriggerAnimation);
    }

    public void TriggerAnimation(PointerEventData eventData)
    {
        if (IsTouchingTarget(eventData))
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
            PlayGlueSound();
        });
        s.Join(transform.DOMove(_endPos.position, _glueSound.length));
        s.Join(_glueLineImage.DOFillAmount(1f, _glueSound.length));
        s.AppendCallback(() =>
        {
            _objective?.CompleteObjective();
            gameObject.SetActive(false);
        });
    }
    
    private void PlayGlueSound() => SoundManager.Instance.PlaySFX(_glueSound, default, 0.5f);
}
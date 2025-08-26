using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Level1
{
    public class Zipper : AUIBehaviour, IDropTarget {
    [SerializeField] private Objective _objective;
    [SerializeField] private Image _zipperLineImage;
    [SerializeField] private AudioClip _zipper1Sound;
    [SerializeField] private AudioClip _zipper2Sound;

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
            GetComponent<Image>().enabled = false;
        });
        if (_zipperLineImage.fillAmount < 0.5f)
        {
            s.Join(_zipperLineImage.DOFillAmount(.5f, _zipper1Sound.length).SetEase(Ease.Linear));
            s.JoinCallback(() => SoundManager.Instance.PlaySFX(_zipper1Sound, default, 0.5f));
        }
        else
        {
            s.Join(_zipperLineImage.DOFillAmount(1f, _zipper2Sound.length).SetEase(Ease.Linear));
            s.JoinCallback(() => SoundManager.Instance.PlaySFX(_zipper2Sound, default, 0.5f));
        }
        s.AppendCallback(() =>
        {
            _objective?.CompleteObjective();
            gameObject.SetActive(false);
        });
    }
}
}
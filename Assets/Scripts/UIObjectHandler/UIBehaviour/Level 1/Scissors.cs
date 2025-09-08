using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Level2
{
    public class Scissors : AUIBehaviour, IDropTarget {
        [SerializeField] private List<Image> _disabledImages;
        [SerializeField] private List<Image> _enabledImages;
        [SerializeField] private AudioClip _cutSound;

        [SerializeField] private Objective _objective;
        [SerializeField] private TungTungBoy _tungTungBoy;

        private DraggableUI _draggableUI;
        private Animator _animator;

        protected override void OnEnable()
        {
            base.OnEnable();
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
            _draggableUI = GetComponent<DraggableUI>();
            _draggableUI.OnDropped.AddListener(CutPicture);
        }

        protected void OnDisable()
        {
            _draggableUI.OnDropped.RemoveListener(CutPicture);
        }

        private void CutPicture(PointerEventData eventData)
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
                _animator.enabled = true;
                _animator.SetTrigger("Cut");
                SoundManager.Instance.PlaySFX(_cutSound, default, 0.5f);
                Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).length);
            }).AppendInterval(_animator.GetCurrentAnimatorStateInfo(0).length);
            s.AppendCallback(() =>
            {
                foreach (var img in _enabledImages)
                    img.gameObject.SetActive(true);
                foreach (var img in _disabledImages)
                    img.gameObject.SetActive(false);
            });
            _objective?.CompleteObjective();
        }
    }
}
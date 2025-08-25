using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class Killer : MonoBehaviour
    {
        [System.Serializable]
        public struct DialogueForFloor
        {
            public string FailDialogue;
            public string CompleteDialogue;
        }

        [Header("References")]
        [SerializeField] private List<Transform> _doorList;
        [SerializeField] private Transform _dialogtransform;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private FloorMover _floorMover;

        [Header("Parameters")]
        [SerializeField] private Ease _easeType = Ease.Linear;
        [SerializeField] private Transform _targetRight;
        [SerializeField] private float _killerMoveRightDuration;
        [SerializeField] private Transform _targetLeft;
        [SerializeField] private float _killerMoveLeftDuration;
        [SerializeField] private Transform _targetCenter;
        [SerializeField] private float _killerMoveCenterDuration;
        [SerializeField] private DialogueForFloor[] dialogueForFloors = new DialogueForFloor[4];
        [Header("Failure")]
        [SerializeField] private Image _backgroundImage;

        [Header("Animations")]
        [SerializeField, SpineAnimation] private string _idleAnimation;
        [SerializeField, SpineAnimation] private string _idle2Animation;
        [SerializeField, SpineAnimation] private string _walkAnimation;
        [SerializeField, SpineAnimation] private string _getCaughtAnimation;

        private SkeletonGraphic _skeletonGraphic;

        private void Awake()
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
        }

        public void SetWalkAnim()
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _walkAnimation, true);
        }

        public void SetIdleAnim()
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _idleAnimation, true);
        }

        public void SetIdle2Anim()
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _idle2Animation, true);
        }

        public void SetGetCaughtAnim()
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _getCaughtAnimation, true);
        }

        public void ObjectivesCompleted()
        {
            var killerScale = transform.localScale;
            int index = _floorMover.CurrentFloorIndex;

            // == Execute Action Sequence ==
            if (!_floorMover.IsLastFloor)
            {

                Sequence s = DOTween.Sequence();
                s.AppendCallback(() =>
                {
                    TouchManager.Instance.DisableEventSystem();
                    _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                });
                s.Append(transform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
                s.JoinCallback(() => SetWalkAnim());
                s.AppendCallback(() =>
                {
                    SetIdle2Anim();
                });
                s.AppendCallback(() =>
                {
                    _dialogtransform.gameObject.SetActive(true);
                    _dialogueText.text = dialogueForFloors[index].CompleteDialogue;
                })
                .AppendInterval(1f);
                s.AppendCallback(() => _dialogtransform.gameObject.SetActive(false));
                s.AppendCallback(() => transform.FlipX());
                s.Append(transform.DOMove(_targetLeft.position, _killerMoveLeftDuration).SetEase(_easeType));
                s.JoinCallback(() => SetWalkAnim());
                s.Append(_floorMover.MoveToNextFloor());
                s.JoinCallback(() => transform.FlipX());
                s.Append(transform.DOMove(_targetCenter.position, _killerMoveCenterDuration).SetEase(_easeType));
                s.AppendCallback(() => SetIdleAnim());
                s.AppendCallback(() => TouchManager.Instance.EnableEventSystem());
            }
            else
            {
                LevelsManager.Instance.OnCurrentLevelCompleted?.Invoke();
                _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() =>
                {
                    TouchManager.Instance.DisableEventSystem();
                    _dialogtransform.gameObject.SetActive(true);
                    _dialogueText.text = dialogueForFloors[index].CompleteDialogue;
                })
                .AppendInterval(1f);
                s.AppendCallback(() => _dialogtransform.gameObject.SetActive(false));
                s.SetDelay(0.35f);
                s.AppendCallback(() =>
                {
                    SetGetCaughtAnim();
                    TouchManager.Instance.EnableEventSystem();
                });
            }
        }

        public void ObjectivesFail()
        {
            var killerScale = transform.localScale;
            int index = _floorMover.CurrentFloorIndex;

            // == Execute Action Sequence ==
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                TouchManager.Instance.DisableEventSystem();
                _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
            });
            s.Append(transform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
            s.JoinCallback(() => SetWalkAnim());
            s.AppendCallback(() =>
            {
                SetIdle2Anim();
            });
            s.AppendCallback(() =>
            {
                _dialogtransform.gameObject.SetActive(true);
                _dialogueText.text = dialogueForFloors[index].FailDialogue;
            }).AppendInterval(1f);
            s.AppendCallback(() => _backgroundImage.gameObject.SetActive(true));
            s.Append(_backgroundImage.DOFade(1, 1f));
            s.AppendCallback(() => TouchManager.Instance.EnableEventSystem());

            Debug.Log("Mission Fail");
            LevelsManager.Instance.OnCurrentLevelFailed?.Invoke();
        }
    }
}
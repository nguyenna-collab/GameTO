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

        [Header("Sound")]
        [SerializeField] private AudioClip _failClip;
        [SerializeField] private AudioClip _successClip;
        [SerializeField] private AudioClip _openDoorClip;
        [SerializeField] private AudioClip _girlScreamClip;
        [SerializeField] private AudioClip _knockknockClip;
        [SerializeField] private AudioClip _manScreamClip;

        [Header("Animations")]
        [SerializeField, SpineAnimation] private string _idleAnimation;
        [SerializeField, SpineAnimation] private string _idle2Animation;
        [SerializeField, SpineAnimation] private string _walkAnimation;
        [SerializeField, SpineAnimation] private string _getCaughtAnimation;

        private SkeletonGraphic _skeletonGraphic;

        public bool IsInFloorTransition { get; private set; }

        private void Awake()
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
        }

        void Start()
        {
            EnterFloor();
        }

        public void SetWalkAnim()
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _walkAnimation, true);
        }

        public void SetIdleAnim(bool loop = true)
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _idleAnimation, loop);
        }

        public void SetIdle2Anim(bool loop = false)
        {
            _skeletonGraphic.AnimationState.SetAnimation(0, _idle2Animation, loop);
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
                    IsInFloorTransition = true;
                    _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                    if (_openDoorClip != null)
                    {
                        SoundManager.Instance.PlaySFX(_openDoorClip);
                    }
                }).AppendInterval(0.35f);
                // s.Append(transform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
                // s.JoinCallback(() => SetWalkAnim());
                // s.AppendCallback(() =>
                // {
                //     SetIdle2Anim();
                // });
                s.AppendCallback(() =>
                {
                    _dialogtransform.gameObject.SetActive(true);
                    _dialogueText.text = dialogueForFloors[index].CompleteDialogue;
                    if (_successClip != null)
                    {
                        SoundManager.Instance.PlaySFX(_successClip);
                    }
                })
                .AppendInterval(1f);
                s.AppendCallback(() => _dialogtransform.gameObject.SetActive(false));
                s.AppendCallback(() => transform.FlipX());
                s.Append(transform.DOMove(_targetLeft.position, _killerMoveLeftDuration).SetEase(_easeType));
                s.JoinCallback(() => SetWalkAnim());
                s.Append(_floorMover.MoveToNextFloor());
                s.JoinCallback(() => transform.FlipX());
                s.AppendCallback(() => EnterFloor());
                s.AppendCallback(() => IsInFloorTransition = false);
            }
            else
            {
                LevelsManager.Instance.OnCurrentLevelCompleted?.Invoke();
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() =>
                {
                    IsInFloorTransition = true;
                    _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                    _dialogtransform.gameObject.SetActive(true);
                    _dialogueText.text = dialogueForFloors[index].CompleteDialogue;
                })
                .AppendInterval(0.5f);
                s.AppendCallback(() => _dialogtransform.gameObject.SetActive(false));
                s.AppendCallback(() =>
                {
                    if (_manScreamClip != null)
                    {
                        SoundManager.Instance.PlaySFX(_manScreamClip);
                    }
                    SetGetCaughtAnim();
                }).AppendInterval(2f);
                s.AppendCallback(() =>
                {
                    IsInFloorTransition = false;
                    UIManager.Instance.ShowDialog("LevelResult", new LevelResultProperties(LevelsManager.Instance.CurrentLevelData.Icon, true));
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
                IsInFloorTransition = true;
                _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                if (_openDoorClip != null)
                {
                    SoundManager.Instance.PlaySFX(_openDoorClip);
                }
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
                if (_failClip != null)
                {
                    SoundManager.Instance.PlaySFX(_failClip);
                }
            }).AppendInterval(1f);
            s.AppendCallback(() =>
            {
                _backgroundImage.gameObject.SetActive(true);
                if (_girlScreamClip != null)
                {
                    SoundManager.Instance.PlaySFX(_girlScreamClip);
                }
            });
            s.Append(_backgroundImage.DOFade(1, 1f));
            s.AppendCallback(() =>
            {
                UIManager.Instance.ShowDialog("LevelResult", new LevelResultProperties(LevelsManager.Instance.CurrentLevelData.Icon, false));
                IsInFloorTransition = false;
            });
            LevelsManager.Instance.OnCurrentLevelFailed?.Invoke();
        }

        private Sequence EnterFloor()
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMove(_targetCenter.position, _killerMoveCenterDuration).SetEase(_easeType));
            s.JoinCallback(() => SetWalkAnim());
            s.AppendCallback(() =>
            {
                SetIdleAnim();
            }).AppendInterval(0.3f);
            s.AppendCallback(() =>
            {
                if (_knockknockClip != null)
                {
                    SoundManager.Instance.PlaySFX(_knockknockClip);
                }
            }).AppendInterval(_knockknockClip.length);
            s.AppendCallback(() => SetIdle2Anim(true));
            return s;
        }
    }
}
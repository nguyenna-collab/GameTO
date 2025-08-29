using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

namespace Level2
{
    public class TungTungBoy : MonoBehaviour
    {
        [Header("Spine Animation")]
        [SerializeField, SpineAnimation] private string _openKeyDoorName;
        [SerializeField, SpineAnimation] private string _buonIaName;
        [SerializeField, SpineAnimation] private string _walkName;
        [SerializeField, SpineAnimation] private string _iaName;
        [SerializeField, SpineAnimation] private string _doWorkoutName;
        [SerializeField, SpineAnimation] private string _catName;
        [SerializeField, SpineAnimation] private string _wcName;

        [Header("Sound")]
        [SerializeField] private AudioClip _keyClip;
        [SerializeField] private AudioClip _breakClip;

        [Space(20)]
        [SerializeField] private Doors _doors;
        [SerializeField] private BubbleTeaGirl _bubbleTeaGirl;
        [SerializeField] private Transform _faceIdTransform;
        [SerializeField] private Image _catImage;
        [SerializeField] private Sprite _catSprite;
        [SerializeField] private Transform _catPosition;
        [SerializeField] private ParticleSystem _woodBreakPSPrefab;
        [SerializeField] private Transform _zipoFire;
        [SerializeField] private Transform _wcPosition;
        [SerializeField] private ObjectiveSO _potatoObjective;
        [SerializeField] private ObjectiveSO _wireObjective;
        [SerializeField] private Transform _scanLight;

        public UnityEvent OnFartObjectiveCompleted;

        private SkeletonGraphic _skeletonGraphic;
        private RectTransform _rectTransform;

        public bool IsWearingFaceId { get; set; }

        private void Awake()
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void TriggerOpenKeyDoorAnimation()
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                _skeletonGraphic.AnimationState.SetAnimation(0, _openKeyDoorName, true);
                SoundManager.Instance.PlaySFX(_keyClip);
            });
            s.AppendInterval(1.5f);
            s.AppendCallback(() =>
            {
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
                _skeletonGraphic.AnimationState.SetAnimation(0, _buonIaName, true);
                _doors.OpenDoor(0);
            });
        }

        public void TriggerOpenWoodDoorAnimation(Objective faceIdObjective)
        {
            Sequence s = DOTween.Sequence();
            float animationDuration = _skeletonGraphic.Skeleton.Data.FindAnimation(_doWorkoutName).Duration;
            s.AppendCallback(() =>
            {
                _skeletonGraphic.AnimationState.SetAnimation(0, _doWorkoutName, false);
            }).AppendInterval(animationDuration);
            s.AppendCallback(() =>
            {
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
                _skeletonGraphic.AnimationState.SetAnimation(0, _buonIaName, true);
                ParticleSystemManager.Instance.PlayParticles(_woodBreakPSPrefab, gameObject.transform.parent, _doors.DoorList[1].position, default, true);
                SoundManager.Instance.PlaySFX(_breakClip);
                _doors.OpenDoor(1, () =>
                {
                    TriggerOpenFaceDoor(faceIdObjective);
                });
            });
        }

        public void TriggerOpenFaceDoor(Objective faceIdObjective)
        {
            if (_doors.CurrentDoorIndex != 2 || !IsWearingFaceId) return;
            _scanLight.gameObject.SetActive(true);
            Sequence s = DOTween.Sequence();
            s.Append(_scanLight.DORotate(_scanLight.localEulerAngles - new Vector3(0, 0, 30), 1f)).SetEase(Ease.Linear);
            s.AppendCallback(() =>
            {
                _scanLight.gameObject.SetActive(false);
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
                _faceIdTransform.gameObject.SetActive(false);
                _doors.OpenDoor(2);
                IsWearingFaceId = false;
                faceIdObjective.CompleteObjective();
            });
        }

        public void TriggerOpenCatDoor()
        {
            if (_doors.CurrentDoorIndex != 3) return;
            var _animDuration = _skeletonGraphic.Skeleton.Data.FindAnimation(_catName).Duration;
            _catImage.sprite = _catSprite;
            _catImage.transform.position = _catPosition.position;

            Sequence s = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                s.AppendCallback(() =>
                {
                    _skeletonGraphic.AnimationState.SetAnimation(0, _catName, false);
                    _doors.ChangePaperDoorSprite(i);
                });
                s.Join(_catImage.transform.DOMoveY(_catPosition.position.y + .5f, _animDuration).SetLoops(2, LoopType.Yoyo));
                s.AppendInterval(_animDuration);
            }
            s.AppendCallback(() =>
            {
                _skeletonGraphic.AnimationState.SetAnimation(0, _catName, false);
                _doors.ChangeDoorToMetalSprite();
            });
            s.Join(_catImage.transform.DOMoveY(_catPosition.position.y + .5f, _animDuration).SetLoops(2, LoopType.Yoyo));
            s.AppendInterval(_animDuration);
            s.AppendCallback(() =>
            {
                _catImage.gameObject.SetActive(false);
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
                _skeletonGraphic.AnimationState.SetAnimation(0, _buonIaName, true);
            });
            s.Append(_doors.OpenDoor(3));
        }

        public void TriggerOpenPuzzleDoor()
        {
            if (_doors.CurrentDoorIndex != 4) return;
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
                _doors.OpenDoor(4);
            });
        }

        public void TriggerOpenIceDoor()
        {
            Sequence s = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                s.Append(_zipoFire.DOMoveY(_zipoFire.position.y + .1f, .2f).SetLoops(2, LoopType.Yoyo)).SetEase(Ease.Linear);
            }
            s.AppendCallback(() =>
            {
                _zipoFire.gameObject.SetActive(false);
                _rectTransform.DOMove(_rectTransform.position + new Vector3(.1f, .1f, 0), 1f);
            });
            s.Append(_doors.OpenDoor(5));
            s.AppendCallback(() => TriggerOpenWcDoor());
        }

        public void TriggerOpenWcDoor()
        {
            if (_doors.CurrentDoorIndex == 6 && _potatoObjective.IsCompleted && _wireObjective.IsCompleted)
            {
                OnFartObjectiveCompleted?.Invoke();
                Sequence s = DOTween.Sequence();
                s.SetDelay(1f);
                s.AppendCallback(() =>
                {
                    _bubbleTeaGirl.GetComponent<MoveObjectSequencer>().MoveObjects();
                    _skeletonGraphic.AnimationState.SetAnimation(0, _walkName, true);
                    _rectTransform.DOMove(_wcPosition.position, 3f).SetEase(Ease.Linear);
                    _doors.OpenDoor(6);
                }).AppendInterval(3f);
                s.AppendCallback(() =>
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    _skeletonGraphic.AnimationState.SetAnimation(0, _wcName, true);
                }).AppendInterval(2f);
                s.AppendCallback(() =>
                {
                    _skeletonGraphic.AnimationState.SetAnimation(0, _iaName, true);
                });
                s.Join(transform.DOMoveY(transform.position.y + 10f, 1f));
                s.AppendCallback(() =>
                {
                    LevelsManager.Instance.OnCurrentLevelCompleted.Invoke();
                });
            }
        }
    }
}
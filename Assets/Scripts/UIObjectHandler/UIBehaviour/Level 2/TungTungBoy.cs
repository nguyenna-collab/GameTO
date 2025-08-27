using UnityEngine;
using Spine.Unity;
using DG.Tweening;

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

        [Space(20)]
        [SerializeField] private Doors _doors;
        [SerializeField] private AudioClip _keyClip;

        private SkeletonGraphic _skeletonGraphic;
        private void Awake()
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
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
                _doors.OpenDoors(0);
                transform.DOMove(transform.position + new Vector3(30f, 30f, 0), 1f);
                _skeletonGraphic.AnimationState.SetAnimation(0, _buonIaName, true);
            });
        }
    }
}
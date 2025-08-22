using Spine.Unity;
using UnityEngine;

namespace Level1
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class Killer : MonoBehaviour
    {
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
    }
}
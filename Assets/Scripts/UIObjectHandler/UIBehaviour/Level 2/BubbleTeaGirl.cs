using UnityEngine;
using Spine.Unity;

namespace Level2
{
    public class BubbleTeaGirl : MonoBehaviour
    {
        [Header("Spine Animation")]
        [SpineAnimation] public string EatName;
        [SpineAnimation] public string FartName;
        [SpineAnimation] public string SlapName;
        [SpineAnimation] public string WcName;

        [Space(20)]
        [SerializeField] private ObjectiveSO _potatoObjective;
        [SerializeField] private ParticleSystem _fanToxicSmokePS;

        private bool _isEating;

        public bool IsEating
        {
            get
            {
                return _isEating;
            }
            set
            {
                _isEating = value;
                if (value == true && !_potatoObjective.IsCompleted)
                {
                    _fanToxicSmokePS.gameObject.SetActive(true);
                    _fanToxicSmokePS.Play();
                }
            }
        }


        public void PlayAnimation(string animationName)
        {
            SkeletonGraphic skeletonGraphic = GetComponent<SkeletonGraphic>();
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, true);
        }
    }
}